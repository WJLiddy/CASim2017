using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
public class Sock
{
    Socket sender;
    string buf = "";

    public Sock()
    {
        // Connect to a remote device.  
        try
        {
            // Create a TCP/IP  socket.  
            sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect("52.14.199.232", 44320);
            sender.ReceiveTimeout = 10; // 10 ms
        }
        catch (Exception e)
        {
            //("Unexpected exception : {0}", e.ToString());
        }
    }
    
    public void Submit(string json)
    {
        sender.Send(Encoding.ASCII.GetBytes("upload    "));
        string num = json.Length.ToString();

        sender.Send(Encoding.ASCII.GetBytes(num));
        for (int i = num.Length; i != 10; i++)
        {
            sender.Send(Encoding.ASCII.GetBytes(" "));
        }
        sender.Send(Encoding.ASCII.GetBytes(json));
    }

    /**
    // tries to read for 0.1 sec. returns NULL if nothing read.
    public SimpleJSON.JSONNode TryRecvJSON()
    {
        try
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024 * 1024];
            // Receive the response from the remote device.  
            int bytesRec = sender.Receive(bytes);
            buf += Encoding.ASCII.GetString(bytes, 0, bytesRec);

            if (buf.Contains("\n"))
            {
                Debug.Log("recv JSON from server!");
                var json = SimpleJSON.JSON.Parse(buf);
                buf = "";
                return json;
            }
            return null;
        }
        catch
        {
            // Did not hear from server.
            return null;
        }
    }
    */

    public void Close()
    {
        // Release the socket.  
        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
    }

}