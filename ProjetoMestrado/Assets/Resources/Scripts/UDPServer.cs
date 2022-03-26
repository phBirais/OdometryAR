using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class UDPServer : MonoBehaviour
{
    // Start is called before the first frame update
    //Client uses as receive udp client
    UdpClient receiver;
    void Start()
    {
        /*
        receiver = new UdpClient(44445);
        try
        {
            receiver.BeginReceive(new AsyncCallback(recv), null);
        }
        catch (Exception e)
        {
            //MessageBox.Show(e.ToString());
            Debug.Log("error");
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        //ReceiveMessage();
        /*
        UdpClient udpClient = new UdpClient("192.168.18.37", 44444);
        Byte[] sendBytes = Encoding.ASCII.GetBytes("Lstart");
        try
        {
            udpClient.Send(sendBytes, sendBytes.Length);
            Debug.Log("mandou");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        Thread.Sleep(3000);*/

    }
    //CallBack
    private void recv(IAsyncResult res)
    {
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 44445);
        byte[] received = receiver.EndReceive(res, ref RemoteIpEndPoint);

        //Process codes

        Debug.Log(Encoding.UTF8.GetString(received));
        receiver.BeginReceive(new AsyncCallback(recv), null);
    }
    private async Task ReceiveMessage()
    {
        using (var udpClient = new UdpClient(44445))
        {
            while (true)
            {
                var receivedResult = await udpClient.ReceiveAsync();
                Debug.Log((Encoding.ASCII.GetString(receivedResult.Buffer)));
            }
        }
    }

    public void StartRobot()
    {
        UdpClient Client = new UdpClient();
        IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 44445);
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




}

