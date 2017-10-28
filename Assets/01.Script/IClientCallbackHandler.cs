
namespace Shrimp.Network
{
    public interface IClientCallbackHandler
    {
        void onConnectFailed();
        void onConnectSucceed();
        void onPacketReceive(short nPacketType, byte[] vPacket);
        void onDisconnect();
    }
}