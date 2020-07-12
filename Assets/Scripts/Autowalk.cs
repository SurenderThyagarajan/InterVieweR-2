using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class Autowalk : MonoBehaviour
{
    public AudioSource audioPlay;
    public Text display;
    public bool socketReady = false;
    public Transform vrcamera;
    public float toggle =30.0f;
    public float speed = 3.0f;
    public bool moveForward;
    public TcpClient mySocket;
    public NetworkStream theStream;
    public StreamWriter theWriter; 
    
    public CharacterController cc;
    public Button subResBtn;
    public Button subFire;
    // Start is called before the first frame update
    void Start()
    {
       
        cc = GetComponentInParent<CharacterController>();
        audioPlay = gameObject.GetComponent<AudioSource>();
        subResBtn = gameObject.GetComponent<Button>();
        subFire = gameObject.GetComponent<Button>();
        
       
    }

    // Update is called once per frame
    void Update()
    {
        if (vrcamera.eulerAngles.x >=  toggle  && vrcamera.eulerAngles.x < 90.0f)
        {
            moveForward = true;
            System.Console.WriteLine("Hello");
        }
        else
            moveForward = false;
        if (moveForward)
        {
            Vector3 fwd = vrcamera.TransformDirection(Vector3.forward);
            cc.SimpleMove(fwd * speed);
        }
        else
        {
            Vector3 fwd = vrcamera.TransformDirection(Vector3.forward);
            cc.SimpleMove(fwd * 0);
        }
    }

  
    public void OnTriggerEnter(Collider other)
    {
        speed = 0.0f;

    }
    

    public void buttonClick()
    {
        setupSocket();
        
    }
    
    public void submitBtn()
    {
        try
        {
            mySocket = new TcpClient("192.168.137.1", 65432);
            theStream = mySocket.GetStream();
            theWriter = new StreamWriter(theStream);

            socketReady = true;
            writeSocket("submit");
        }
        catch (Exception e)
        {
            Debug.Log("Socket error:" + e);
        }
    }

    public void queBtn()
    {
        try
        {
            mySocket = new TcpClient("192.168.137.1", 65432);
            theStream = mySocket.GetStream();
            theWriter = new StreamWriter(theStream);

            socketReady = true;
            writeSocket("recieve");
        }
        catch (Exception e)
        {
            Debug.Log("Socket error:" + e);
        }
    }
    public void setupSocket()
    {
        try
        {
            mySocket = new TcpClient("192.168.137.1", 65432);
            theStream = mySocket.GetStream();
            theWriter = new StreamWriter(theStream);
            
            socketReady = true;
            writeSocket("record");
        }
        catch (Exception e)
        {
            Debug.Log("Socket error:" + e);
        }
    }

    public void writeSocket(string theLine)
    {
        if (!socketReady)
            return;
        String tmpString = theLine;
        Debug.Log(tmpString);
        theWriter.Write(tmpString);
        theWriter.Flush();
        mySocket.Close();
        Thread.Sleep(8000);
        displayMsg();
    }

    public void displayMsg()
    {
        Debug.Log("After t secs");
        string msg = "";
        connection cn = new connection();

        msg = cn.Connection();
        Debug.Log(msg);

        display.text = msg;
        StartCoroutine(Music(msg));
    }
    IEnumerator  Music(String msg)
       {
       Debug.Log("Hello world");
           Regex regex = new Regex("\\s+");
           string res = regex.Replace(msg, "+");
           string url = "https://api.voicerss.org/?key=6a9b31137cd84b1d98970e0ffc078d50&hl=en-gb&f=44khz_16bit_stereo&src=" + res;
           WWW www = new WWW(url);
           yield return www;
           audioPlay.clip = www.GetAudioClip(false, true, AudioType.WAV);
           audioPlay.Play();
       }





}
