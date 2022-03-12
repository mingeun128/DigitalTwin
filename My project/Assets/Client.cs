using System;
using System.IO;
using System.IO.Pipes;
using UnityEngine;


public class Client : MonoBehaviour
{
    private static string pipe1Name = "clientpipe";
    private static string pipe2Name = "serverpipe";
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            using (var client = new NamedPipeClientStream(pipe1Name))
            {
                client.Connect(300);
                var writer = new StreamWriter(client);
                var request = "Hi, server.";
                writer.WriteLine(request);
                writer.Flush();
            }

        }
        if (Input.GetKey(KeyCode.D))
        {
            using (var server = new NamedPipeClientStream(pipe2Name))
            {
                server.Connect(300);
                var reader = new StreamReader(server);
                var response = reader.ReadLine();
                Debug.Log("Response from server: " + response);
            }

        }
    }
    
}