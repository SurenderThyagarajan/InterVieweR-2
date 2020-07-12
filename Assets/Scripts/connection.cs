using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class connection : MonoBehaviour
{

   
    public string word = "";

    // Start is called before the first frame update

    public void Start()
    {
        
        

    }


    public String Connection()
    {
        String msg = "";
        Debug.Log("Hello again");
        try
        {
            IPAddress ipAddress = IPAddress.Parse("192.168.137.1");
            Debug.Log(ipAddress.ToString());
            Socket sender = new Socket(ipAddress.AddressFamily,
                      SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 1024);
            sender.Connect(localEndPoint);
            Debug.Log(ipAddress.ToString() + " Connected");

            byte[] messageReceived = new byte[1024];
            int byteRecv = sender.Receive(messageReceived);
            msg = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);

            Debug.Log("connected");
            Debug.Log(msg);
           
            return msg;

        }


        catch (Exception e)
        {
            Debug.Log(e.ToString());
            return "Please retry the action";

        }
    }
    
       
        
    

    // Update is called once per frame
    public void Update()
    {
        
    }
}
