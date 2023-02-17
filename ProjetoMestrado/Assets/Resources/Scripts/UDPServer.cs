using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine.UI;

public class UDPServer : MonoBehaviour
{
    public int port = 44445;
    public bool gameStatus = false;

    public float distance = 0;

    public VirtualBotSpeedController virtualBotSpeedController;
    public InputField leftSpeedInput;
    public InputField rightSpeedInput;

    float perimeter = 0.1036725f; //meters//

    public int lSpeed = 300;
    public int rSpeed = 300;

    public float[] realBotValues;

    Thread st;
    UdpClient listener;
    IPEndPoint groupEP;
    void Start()
    {
        Debug.Log("Inicia Thread");
        virtualBotSpeedController = GameObject.Find("VirtualRobot").GetComponent<VirtualBotSpeedController>();  
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
                //print(returnData);
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
        realBotValues = Array.ConvertAll(dadosRecebidos, float.Parse);

        //print(realBotValues[0]);
        Debug.Log("valores: " + realBotValues[1]);

        //calcula a distancia percorrida
        float dist  = ((realBotValues[2] / 440) * perimeter) + ((realBotValues[1] / 440) * perimeter);

        //Calcula a média da distância
        dist /= 2;

        return dist;
    }

    public void StartRobot()
    {
        //float leftSpeed = virtualBotSpeedController.leftWheelSpeed;
        //float rightSpeed = virtualBotSpeedController.rightWheelSpeed;
        int leftSpeed = 0;
        int rightSpeed = 0;
        gameStatus = true;

        if (int.TryParse(leftSpeedInput.text, out int result))
        {
            leftSpeed = result;
        }

        if (int.TryParse(rightSpeedInput.text, out int result2))
        {
            rightSpeed = result2;
        }

        //tranformar velocidade em pwm
        // RPM 0 - 475 --> PWM 0 - 1023
        leftSpeed = (leftSpeed * 1023 / 475);
        rightSpeed = (rightSpeed * 1023 / 475);

        Debug.Log("leftReal: " + leftSpeed);
        Debug.Log("rightReal: " + rightSpeed);

        //mandar as velocidade para os robos em pwm ou porcentagem

        UdpClient Client = new UdpClient();
        IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.18.57"), port); //IP do esp32
        //IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.201.227"), port); //IP do esp32 com celular roteando
        //IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 44445);
        byte[] bytes = Encoding.ASCII.GetBytes("start,"+leftSpeed+","+rightSpeed);

        try
        {
            Client.Send(bytes, bytes.Length, ip);

            Debug.Log("mandouStart");
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
        IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.18.57"), port);
        //IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.201.227"), port);//IP celular roteando
        //IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 44445);
        byte[] bytes = Encoding.ASCII.GetBytes("stop");
        gameStatus = false;

        try
        {
            //Debug.Log("mandandoStop");
            Client.Send(bytes, bytes.Length, ip);

            Debug.Log("mandou Stop");
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
        IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.18.57"), port);
        //IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.201.227"), port); //IP celular roteando
        //IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 44445);
        byte[] bytes = Encoding.ASCII.GetBytes("pause");
        gameStatus = false;

        try
        {
            //Debug.Log("mandandoStop");
            Client.Send(bytes, bytes.Length, ip);

            Debug.Log("mandou Pause");
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

