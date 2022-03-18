using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using UnityEngine;
using System.Runtime.InteropServices;



public class Client : MonoBehaviour
{
    NamedPipeServerStream pipe;
    NamedPipeServerStream pipe2;
    string str="";

    void Start()
    {
        Debug.Log("SERVER START");
        pipe = new NamedPipeServerStream("DEVSPipe", PipeDirection.InOut);
        pipe2 = new NamedPipeServerStream("DEVSPipe2", PipeDirection.InOut);
        Thread t1 = new Thread(new ThreadStart(GetPipeData));
        Thread t2 = new Thread(new ThreadStart(SendPipeData));
        t1.IsBackground = true;
        t1.Start();
        t2.IsBackground = true;
        t2.Start();
    }
    void Update()
    {
        if (str.Length > 0)
        {
            Debug.Log(str);
            str = "";
        }
    }
    private void GetPipeData()
    {
        while (true)
        {
            if (!pipe.IsConnected)
            {
                pipe.Close();
                pipe = new NamedPipeServerStream("DEVSPipe", PipeDirection.InOut);
                pipe.WaitForConnection();
            }
            byte[] sr = new byte[1024];
            pipe.Read(sr, 0, 1024);
            str = System.Text.Encoding.Default.GetString(sr);
            str = str.Split('\0')[0];
        }
        
    }
    private void SendPipeData()
    {
        while (true)
        {
            if (!pipe2.IsConnected)
            {
                pipe2.Close();
                pipe2 = new NamedPipeServerStream("DEVSPipe2", PipeDirection.InOut);
                pipe2.WaitForConnection();
            }
            StreamWriter sw = new StreamWriter(pipe2);
            sw.AutoFlush = true;
            String strHello = "Hello";
            //IntPtr pStr = Marshal.StringToHGlobalUni(strHello);
            sw.WriteLine(strHello);
            //Thread.Sleep(2000);
        }

    }
}