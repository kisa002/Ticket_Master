
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Shrimp.Network
{
    public class CallbackClient
    {
        public bool IsConnected { get { lock (this.sGlobalLock) { return this.sSocket != null; } } }
        public bool UseNagle { get { return this.bUseNagle; } }
        public bool UseKeepAlive { get { return this.bUseKeepAlive; } }
        public int KeepAliveTime { get { return this.nKeepAliveTime; } }
        public int KeepAliveInterval { get { return this.nKeepAliveInterval; } }

        //Locks.
        private object sGlobalLock = new object();

        private bool bUseNagle;
        private bool bUseKeepAlive;
        private int nKeepAliveTime;
        private int nKeepAliveInterval;
        private Socket sSocket;
        private byte[] vPacketHeader = new byte[6];
        private SocketAsyncEventArgs sConnectEventArgs = new SocketAsyncEventArgs();
        private SocketAsyncEventArgs sSendEventArgs = new SocketAsyncEventArgs();
        private SocketAsyncEventArgs sReceiveEventArgs = new SocketAsyncEventArgs();
        private Queue<byte[]> sClientSendBufferQueue = new Queue<byte[]>();
        private IClientCallbackHandler sClientCallbackHandler;

        public CallbackClient(IClientCallbackHandler sCallbackHandler, bool bEnableNagle, bool bEnableKeepAlive, int nKeepAliveTime, int nKeepAliveInterval)
        {
            this.sClientCallbackHandler = sCallbackHandler;
            this.bUseNagle = bEnableNagle;
            this.bUseKeepAlive = bEnableKeepAlive;
            this.nKeepAliveTime = nKeepAliveTime;
            this.nKeepAliveInterval = nKeepAliveInterval;

            this.sConnectEventArgs.Completed += onConnect;
            this.sSendEventArgs.Completed += onSendPacket;
        }

        public bool connectTo(string sHost, int nPort)
        {
            if (this.IsConnected)
                return false;

            lock (this.sGlobalLock)
            {
                this.sSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.sSocket.NoDelay = !this.bUseNagle;

                byte[] vBuffer = new byte[12];
                Array.Copy(BitConverter.GetBytes(this.bUseKeepAlive ? 1 : 0), 0, vBuffer, 0, sizeof(int));
                Array.Copy(BitConverter.GetBytes(this.nKeepAliveTime), 0, vBuffer, sizeof(int), sizeof(int));
                Array.Copy(BitConverter.GetBytes(this.nKeepAliveInterval), 0, vBuffer, sizeof(int) * 2, sizeof(int));

                try
                {
                    this.sSocket.IOControl(IOControlCode.KeepAliveValues, vBuffer, null);
                }
                catch
                {
                    //Empty.
                }

                this.sConnectEventArgs.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(sHost), nPort);
            }

            try
            {
                if (!this.sSocket.ConnectAsync(this.sConnectEventArgs))
                    this.onConnect(this.sSocket, this.sConnectEventArgs);
            }
            catch
            {
                this.disconnectFrom();
            }

            return true;
        }

        public void disconnectFrom(int nTimeout = 1)
        {
            if (!this.IsConnected)
                return;

            lock (this.sGlobalLock)
            {
                this.sSocket.Shutdown(SocketShutdown.Both);
                this.sSocket.Close(nTimeout);
                this.sSocket = null;
            }
        }

        private void onConnect(object sSender, SocketAsyncEventArgs sEventArgs)
        {
            if (sEventArgs.SocketError != SocketError.Success)
            {
                this.disconnectFrom();
                this.sClientCallbackHandler.onConnectFailed();
                return;
            }

            this.sClientCallbackHandler.onConnectSucceed();

            this.sReceiveEventArgs.SetBuffer(this.vPacketHeader, 0, this.vPacketHeader.Length);
            this.sReceiveEventArgs.Completed -= this.onReceivePacketHeader;
            this.sReceiveEventArgs.Completed -= this.onReceivePacket;
            this.sReceiveEventArgs.Completed += this.onReceivePacketHeader;

            try
            {
                if (!this.sSocket.ReceiveAsync(this.sReceiveEventArgs))
                    this.onReceivePacketHeader(this.sSocket, this.sReceiveEventArgs);
            }
            catch
            {
                this.disconnectFrom();
                this.sClientCallbackHandler.onDisconnect();
            }
        }

        public void sendTo(short nPacketType, byte[] vPacket)
        {
            var vPacketHeader = new byte[6];

            NetworkUtility.toNetwork(nPacketType, vPacketHeader, 0);
            NetworkUtility.toNetwork(vPacket == null ? 0 : vPacket.Length, vPacketHeader, 2);

            lock (this.sSendEventArgs)
            {
                if (this.sSendEventArgs.UserToken != null)
                {
                    this.sClientSendBufferQueue.Enqueue(vPacketHeader);

                    if (vPacket != null || vPacket.Length != 0)
                        this.sClientSendBufferQueue.Enqueue(vPacket);

                    return;
                }

                this.sSendEventArgs.UserToken = this.sSocket;

                if (vPacket != null || vPacket.Length != 0)
                    this.sClientSendBufferQueue.Enqueue(vPacket);
            }

            this.sSendEventArgs.SetBuffer(vPacketHeader, 0, vPacketHeader.Length);

            try
            {
                if (!this.sSocket.SendAsync(this.sSendEventArgs))
                    this.onSendPacket(this.sSocket, this.sSendEventArgs);
            }
            catch
            {
                this.disconnectFrom();
                this.sClientCallbackHandler.onDisconnect();
            }
        }

        private void onSendPacket(object sSender, SocketAsyncEventArgs sEventArgs)
        {
            if (sEventArgs.SocketError != SocketError.Success)
            {
                this.disconnectFrom();
                this.sClientCallbackHandler.onDisconnect();
                return;
            }

            if (sEventArgs.BytesTransferred == 0)
            {
                this.disconnectFrom();
                this.sClientCallbackHandler.onDisconnect();
                return;
            }

            var sClientSocket = (Socket)sSender;

            if (sEventArgs.Offset + sEventArgs.BytesTransferred < sEventArgs.Count)
            {
                sEventArgs.SetBuffer(sEventArgs.Buffer, sEventArgs.Offset + sEventArgs.BytesTransferred, sEventArgs.Count - sEventArgs.BytesTransferred);

                try
                {
                    if (!sClientSocket.SendAsync(sEventArgs))
                        this.onSendPacket(sClientSocket, sEventArgs);
                }
                catch
                {
                    this.disconnectFrom();
                    this.sClientCallbackHandler.onDisconnect();
                }

                return;
            }

            byte[] vPacket;

            lock (sEventArgs)
            {
                if (this.sClientSendBufferQueue.Count == 0)
                {
                    sEventArgs.UserToken = null;
                    return;
                }

                vPacket = this.sClientSendBufferQueue.Dequeue();
            }

            sEventArgs.SetBuffer(vPacket, 0, vPacket.Length);

            try
            {
                if (!sClientSocket.SendAsync(sEventArgs))
                    this.onSendPacket(sClientSocket, sEventArgs);
            }
            catch
            {
                this.disconnectFrom();
                this.sClientCallbackHandler.onDisconnect();
            }
        }

        private void onReceivePacketHeader(object sSender, SocketAsyncEventArgs sEventArgs)
        {
            if (sEventArgs.SocketError != SocketError.Success)
            {
                this.disconnectFrom();
                this.sClientCallbackHandler.onDisconnect();
                return;
            }

            if (sEventArgs.BytesTransferred == 0)
            {
                this.disconnectFrom();
                this.sClientCallbackHandler.onDisconnect();
                return;
            }

            var sClientSocket = (Socket)sSender;

            if (sEventArgs.Offset + sEventArgs.BytesTransferred < sEventArgs.Count)
            {
                sEventArgs.SetBuffer(sEventArgs.Buffer, sEventArgs.Offset + sEventArgs.BytesTransferred, sEventArgs.Count - sEventArgs.BytesTransferred);

                try
                {
                    if (!sClientSocket.ReceiveAsync(sEventArgs))
                        this.onReceivePacketHeader(sClientSocket, sEventArgs);
                }
                catch
                {
                    this.disconnectFrom();
                    this.sClientCallbackHandler.onDisconnect();
                }

                return;
            }

            int nPacketLength = NetworkUtility.toHost32(sEventArgs.Buffer, 2);

            if (nPacketLength < 0)
            {
                this.disconnectFrom();
                this.sClientCallbackHandler.onDisconnect();
                return;
            }
            else if (nPacketLength == 0)
            {
                this.sClientCallbackHandler.onPacketReceive(NetworkUtility.toHost16(sEventArgs.Buffer, 0), null);
                sEventArgs.SetBuffer(this.vPacketHeader, 0, this.vPacketHeader.Length);

                try
                {
                    if (!sClientSocket.ReceiveAsync(sEventArgs))
                        this.onReceivePacketHeader(sClientSocket, sEventArgs);
                }
                catch
                {
                    this.disconnectFrom();
                    this.sClientCallbackHandler.onDisconnect();
                }

                return;
            }

            sEventArgs.SetBuffer(new byte[nPacketLength], 0, nPacketLength);
            sEventArgs.Completed -= this.onReceivePacketHeader;
            sEventArgs.Completed += this.onReceivePacket;

            try
            {
                if (!sClientSocket.ReceiveAsync(sEventArgs))
                    this.onReceivePacket(sClientSocket, sEventArgs);
            }
            catch
            {
                this.disconnectFrom();
                this.sClientCallbackHandler.onDisconnect();
            }
        }

        private void onReceivePacket(object sSender, SocketAsyncEventArgs sEventArgs)
        {
            if (sEventArgs.SocketError != SocketError.Success)
            {
                this.disconnectFrom();
                this.sClientCallbackHandler.onDisconnect();
                return;
            }

            if (sEventArgs.BytesTransferred == 0)
            {
                this.disconnectFrom();
                this.sClientCallbackHandler.onDisconnect();
                return;
            }

            var sClientSocket = (Socket)sSender;

            if (sEventArgs.Offset + sEventArgs.BytesTransferred < sEventArgs.Count)
            {
                sEventArgs.SetBuffer(sEventArgs.Buffer, sEventArgs.Offset + sEventArgs.BytesTransferred, sEventArgs.Count - sEventArgs.BytesTransferred);

                try
                {
                    if (!sClientSocket.ReceiveAsync(sEventArgs))
                        this.onReceivePacket(sClientSocket, sEventArgs);
                }
                catch
                {
                    this.disconnectFrom();
                    this.sClientCallbackHandler.onDisconnect();
                }

                return;
            }

            this.sClientCallbackHandler.onPacketReceive(NetworkUtility.toHost16(this.vPacketHeader, 0), sEventArgs.Buffer);

            sEventArgs.SetBuffer(this.vPacketHeader, 0, this.vPacketHeader.Length);
            sEventArgs.Completed -= this.onReceivePacket;
            sEventArgs.Completed += this.onReceivePacketHeader;

            try
            {
                if (!sClientSocket.ReceiveAsync(sEventArgs))
                    this.onReceivePacketHeader(sClientSocket, sEventArgs);
            }
            catch
            {
                this.disconnectFrom();
                this.sClientCallbackHandler.onDisconnect();
            }
        }
    }
}