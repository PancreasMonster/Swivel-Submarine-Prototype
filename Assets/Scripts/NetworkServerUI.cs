using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Net;
using System.Net.Sockets;


public class NetworkServerUI : MonoBehaviour
{
#if UNITY_ANDROID
    NetworkClient client;
#endif 

    void OnGUI()
    {
#if UNITY_STANDALONE_WIN
        GUI.Box(new Rect(10, Screen.height - 80, 100, 20), LocalIPAddress());
        GUI.Box(new Rect(10, Screen.height - 50, 100, 20), "Status:" + NetworkServer.active);
        GUI.Box(new Rect(10, Screen.height - 20, 100, 20), "Connected:" + NetworkServer.connections.Count);
#endif
#if UNITY_ANDROID
        GUI.Box(new Rect(10, Screen.height - 80, 100, 20), "Status:" + client.isConnected);

        if (!client.isConnected)
        {
            if(GUI.Button(new Rect(10,10, 60, 50), "Connect"))
            {
                Connect();
            }
        }
#endif 
    }

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_STANDALONE_WIN
        NetworkServer.Listen(25000);
#endif
#if UNITY_ANDROID
        client = new NetworkClient();
#endif
    }

    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }

    void Connect()
    {
        #if UNITY_ANDROID
        client.Connect("192.168.0.2", 25000);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
