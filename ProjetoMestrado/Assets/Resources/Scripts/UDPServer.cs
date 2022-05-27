using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPServer : MonoBehaviour
{
    public int port = 44445;

    public float distance = 0;

    float perimeter = 0.1068f; //10 cm

    Thread st;
    UdpClient listener;
    IPEndPoint groupEP;
    void Start()
    {
        Debug.Log("TESTE");
        st = new Thread(StartListener);
        st.Start();
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
                distance = GetDistance(returnData);
            }
            catch (Exception e)
            {
                print("There is an error: " + e.Message);
                //st.Abort();
            }
        }
    }
    public float GetDistance(string msg)
    {
        string[] dadosRecebidos = msg.Split(',');
        //Debug.Log("dados: " + dadosCadastro[0]);
        float[] valoresDouble = Array.ConvertAll(dadosRecebidos, float.Parse);
        //Debug.Log("vetor double: " + valoresDouble[2].ToString());

        float pos = ((valoresDouble[2] / 438) * perimeter) + ((valoresDouble[1] / 438) * perimeter);
        pos /= 2;

        return pos;
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

