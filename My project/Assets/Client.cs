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
    Thread t1;
    Thread t2;
    string str="";
    string car_position = "";
    GameObject car = null;
    private Vector3 carPosition;

    void Start()
    {
        car = GameObject.Find("car 1203 yellow");
        Debug.Log("SERVER START");
        pipe = new NamedPipeServerStream("DEVSPipe", PipeDirection.InOut);
        pipe2 = new NamedPipeServerStream("DEVSPipe2", PipeDirection.InOut);
        t1 = new Thread(new ThreadStart(GetPipeData));
        t2 = new Thread(new ThreadStart(SendPipeData));
        t1.IsBackground = true;
        t1.Start();
        t2.IsBackground = true;
        t2.Start();
    }
    void Update()
    {
        carPosition = car.transform.position;
        car_position = carPosition.ToString();
        if (str.Length > 0)
        {
            Debug.Log(str);
            str = "";
        }
    }
    void OnDestroy()
    {
        t1.Abort();
        t2.Abort();
        //pipe.Close();
        //pipe2.Close();
        
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
            try
            {
                // Read user input and send that to the client process.
                StreamWriter sw = new StreamWriter(pipe2);
                sw.WriteLine(car_position);
                sw.Flush();
            }
            // Catch the IOException that is raised if the pipe is broken
            // or disconnected.
            catch (IOException e)
            {
                Debug.Log(e.Message);
            }
            Thread.Sleep(1000);
        }
        
    }
}