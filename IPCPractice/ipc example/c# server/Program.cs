using System;
using System.Text;
using System.IO;
using System.IO.Pipes;

namespace CSPipe
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SERVER");
            NamedPipeServerStream pipe = new NamedPipeServerStream("DEVSPipe",PipeDirection.InOut);
            
            while (true) {
                if (!pipe.IsConnected)
                {
                    pipe.Close();
                    pipe = new NamedPipeServerStream("DEVSPipe", PipeDirection.InOut);
                    pipe.WaitForConnection();
                }
                byte[] sr = new byte[1024];
                pipe.Read(sr,0, 1024);
                string str = System.Text.Encoding.Default.GetString(sr);
                str = str.Split('\0')[0];
                System.Console.WriteLine(str);
                
            }
        }
    }
}