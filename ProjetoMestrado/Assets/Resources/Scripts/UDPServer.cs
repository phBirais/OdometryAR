using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

public class UDPServer : MonoBehaviour
{
    // Start is called before the first frame update
    //Client uses as receive udp client
    UdpClient receiver;

    public int port = 44445;

    Thread st;
    UdpClient listener;
    IPEndPoint groupEP;
    void Start()
    {
        st = new Thread(StartListener);
        st.Start();
        Debug.Log("TESTE");
    }

    // Update is called once per frame
    void Update()
    {    

    }

    void StartListener()
    {
        //listen on port
        listener = new UdpClient(port);

        //Client's IP
        groupEP = new IPEndPoint(IPAddress.Any, port);

        while (true)
        {
            try
            {               
                byte[] bytes = listener.Receive(ref groupEP);
                string returnData = Encoding.ASCII.GetString(bytes);
                print(returnData);
            }
            catch (Exception e)
            {
                print("There is an error: " + e.Message);
                st.Abort();
            }
        }
    }

    public void StartRobot()
    {
        UdpClient Client = new UdpClient();
        IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.18.59"), port);
        byte[] bytes = Encoding.ASCII.GetBytes("start");

        try
        {
            Client.Send(bytes, bytes.Length, ip);

            Debug.Log("mandou");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        finally
        {
            Client.Close();
        };
    }

    public void StopRobot()
    {
        UdpClient Client = new UdpClient();
        IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 44445);
        byte[] bytes = Encoding.ASCII.GetBytes("stop");

        try
        {
            //Debug.Log("mandandoStop");
            Client.Send(bytes, bytes.Length, ip);

            Debug.Log("mandouStop");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        finally
        {
            Client.Close();
        }
    }

    private void OnApplicationQuit()
    {
        StopRobot();
        listener.Dispose();
        //listener.EndReceive(null,ref groupEP);
        listener.Close();
        st.Abort();
        print("Closing client");
    }



}

