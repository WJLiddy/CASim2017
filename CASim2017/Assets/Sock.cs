using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
public class Sock
{
    Socket sender;

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


    public List<SimpleJSON.JSONNode> Recv()
    {
        sender.Send(Encoding.ASCII.GetBytes("download  "));
        sender.Send(Encoding.ASCII.GetBytes("10        "));
        List<SimpleJSON.JSONNode> list = new List<SimpleJSON.JSONNode>();

        string buf = "";
        for (int prop = 0; prop != 8; prop++)
        {
            Debug.Log("await...");

            while (buf.Length < 10)
            {
                // Data buffer for incoming data.  
                byte[] bytes = new byte[1024 * 1024];
                // Receive the response from the remote device.  
                int bytesRec = 0;
                try
                {
                    bytesRec = sender.Receive(bytes);
                } catch
                {
                    continue;
                }
                buf += Encoding.ASCII.GetString(bytes, 0, bytesRec);
            }

            int o;
            Int32.TryParse(buf.Substring(0, 10), out o);

            Debug.Log("prop has " + o + "bytes");

            buf = buf.Substring(10);


            while (buf.Length < o)
            {
                // Data buffer for incoming data.  
                byte[] bytes = new byte[1024 * 1024];
                // Receive the response from the remote device.  
                int bytesRec = 0;
                try
                {
                    bytesRec = sender.Receive(bytes);
                }
                catch
                {
                    continue;
                }
                buf += Encoding.ASCII.GetString(bytes, 0, bytesRec);
            }
            list.Add(SimpleJSON.JSON.Parse(buf.Substring(0,o)));
            buf = buf.Substring(o);
        }
        return list;
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