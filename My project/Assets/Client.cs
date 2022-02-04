using System;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

public class Client : MonoBehaviour
{
    public string m_Ip = "127.0.0.1";
    public int m_Port = 50001;
    private IPEndPoint m_IpEndPoint;
    public ToServerPacket m_SendPacket = new ToServerPacket();
    public ToClientPacket m_ReceivePacket = new ToClientPacket();
    private bool m_IsConnected;
    private Socket m_Socket;

    void Awake()
    {
        InitSocket();

        if (!m_IsConnected)
            ConnectServer();
    }

    void OnApplicationQuit()
    {
        CloseSocket();
    }

    void Update()
    {
        if (!m_IsConnected)
            ConnectServer();

        else
        {
            Send();
            Receive();
        }
    }

    void InitSocket()
    {
        m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipAddress = IPAddress.Parse(m_Ip);
        m_IpEndPoint = new IPEndPoint(ipAddress, m_Port);

        // SendPacket에 배열이 있으면 선언 해 주어야 함.
        //m_SendPacket.m_IntArray = new int[2];
    }

    void ConnectServer()
    {
        try
        {
            m_Socket.Connect(m_IpEndPoint);
            m_IsConnected = true;
        }

        catch (SocketException ex)
        {
            Debug.Log(ex.Message);
        }
    }

    void CloseSocket()
    {
        m_Socket.Close();
        m_Socket = null;
    }

    void SetSendPacket()
    {
        m_SendPacket.m_BoolVariable = true;
        m_SendPacket.m_IntVariable = 13;
        //m_SendPacket.m_IntArray[0] = 7;
        //m_SendPacket.m_IntArray[1] = 47;
        m_SendPacket.m_FloatlVariable = 2020;
        m_SendPacket.m_StringlVariable = "Coder Zero";
    }


    void Receive()
    {
        int receive = 0;

        try
        {
            byte[] receivedbytes = new byte[512];
            receive = m_Socket.Receive(receivedbytes);

            m_ReceivePacket = ByteArrayToStruct<ToClientPacket>(receivedbytes);
        }

        catch (Exception ex)
        {
            Debug.Log(ex);
            return;
        }

        if (receive > 0)
        {
            DoReceivePacket(); // 받은 값 처리
        }
    }

    void DoReceivePacket()
    {
        Debug.LogFormat($"BoolVariable = {m_ReceivePacket.m_BoolVariable} " +
             $"IntlVariable = {m_ReceivePacket.m_IntVariable} " +
             $"FloatlVariable = {m_ReceivePacket.m_FloatlVariable} " +
             $"StringlVariable = {m_ReceivePacket.m_StringlVariable}");
    }

    void Send()
    {
        try
        {
            SetSendPacket();
            byte[] sendPacket = StructToByteArray(m_SendPacket);
            m_Socket.Send(sendPacket);
        }

        catch (SocketException ex)
        {
            Debug.Log(ex.Message);
        }
    }

    byte[] StructToByteArray(object obj)
    {
        int size = Marshal.SizeOf(obj);
        byte[] arr = new byte[size];
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.StructureToPtr(obj, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }

    T ByteArrayToStruct<T>(byte[] buffer) where T : struct
    {
        int size = Marshal.SizeOf(typeof(T));
        if (size > buffer.Length)
        {
            throw new Exception();
        }

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(buffer, 0, ptr, size);
        T obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);
        return obj;
    }
}