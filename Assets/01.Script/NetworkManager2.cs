
using Shrimp.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkManager : MonoBehaviour, IClientCallbackHandler {

    public static NetworkManager Instance { get { return NetworkManager.sInstance; } }

    private static NetworkManager sInstance;

    private CallbackClient sClient;

    private void Awake()
    {
        NetworkManager.sInstance = this;

        this.sClient = new CallbackClient(this, false, false, 0, 0);
    }

    public void connectToServer(string sServerIP)
    {
        this.sClient.connectTo(sServerIP, 8888);
    }

    public void onConnectFailed()
    {
        //TODO : Implement this method.
    }

    public void onConnectSucceed()
    {
        //TODO : Implement this method.
    }

    public void onDisconnect()
    {
        //TODO : Implement this method.
    }

    public void onPacketReceive(short nPacketType, byte[] vPacket)
    {
        switch(nPacketType)
        {
            case 0:
                //TODO ACTION
                break;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
