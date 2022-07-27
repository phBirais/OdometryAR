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

    float perimeter = 0.1036725f; //

    Thread st;
    UdpClient listener;
    IPEndPoint groupEP;
    void Start()
    {
        Debug.Log("Inicia Thread");
        st = new Thread(StartListener);
        st.Start();
    }

    // Update is called once per frame
    void Update()
    {    

    }

    void StartListener()
    {
        //Ouvindo na porta
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
            }
        }
    }
    public float GetDistance(string msg)
    {
        //recebe os dados por string e separa por vírgula;
        string[] dadosRecebidos = msg.Split(',');
        //Converte os dados recebidos para float;
        float[] valoresDouble = Array.ConvertAll(dadosRecebidos, float.Parse);

        //calcula a distancia percorrida
        float dist  = ((valoresDouble[2] / 440) * perimeter) + ((valoresDouble[1] / 440) * perimeter);

        //Calcula a média da distância
        dist /= 2;

        return dist;
    }

    public void StartRobot()
    {
        UdpClient Client = new UdpClient();
        IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.18.57"), port);
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

    public void PauseRobot()
    {
        UdpClient Client = new UdpClient();
        IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 44445);
        byte[] bytes = Encoding.ASCII.GetBytes("pause");

        try
        {
            //Debug.Log("mandandoStop");
            Client.Send(bytes, bytes.Length, ip);

            Debug.Log("mandouPause");
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

