using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

public class CsvWriter : MonoBehaviour
{
    //referencias a outros scripts;
    public VirtualBotSpeedController virtualrobotController;
    public RobotController robotController;
    public UDPServer udpServer;
    public KalmanFusion kalmanFusion;

    public GameObject virtualBot;

    //tempo de start
    float virtualBotFirstTime = 0, realBotFirstTime = 0, kalmanBotFirstTime = 0;

    string virtualBotFileName = "", realBotFileName = "", kalmanBotFileName = "";

    StreamWriter virtualBotTw, realBotTw, kalmanBotTw;
    void Start()
    {
        //criação do arquivo para dados do robô virtual e real
        virtualBotFileName = Application.dataPath + "/virtualBotData.csv";
        realBotFileName = Application.dataPath + "/realBotData.csv";
        kalmanBotFileName = Application.dataPath + "/kalmanBotData.csv";

        virtualBotTw = new StreamWriter(virtualBotFileName, false);
        virtualBotTw.WriteLine("Time; Xposition; Yposition; Angle; Speed");
        virtualBotTw.Close();

        realBotTw = new StreamWriter(realBotFileName, false);
        realBotTw.WriteLine("HardwareTime; SoftwareTime; Xposition; Yposition; Angle; RightEncoder; LeftEncoder");
        realBotTw.Close();

        kalmanBotTw = new StreamWriter(kalmanBotFileName, false);
        kalmanBotTw.WriteLine("Time; EkX; EodomX; EcamX; Ykx");
        kalmanBotTw.Close();


        print("Gravação csv iniciada");
    }

    void Update()
    {
        if(udpServer.gameStatus == true)
        {
            WriteCSV();            
        }
        else {
            virtualBotFirstTime = Time.time;
            kalmanBotFirstTime = Time.time;
            if(udpServer.realBotValues.Length > 0)
                realBotFirstTime = udpServer.realBotValues[0];
        }
    }

    void WriteCSV()
    {
        //print("entrou");
        virtualBotTw = new StreamWriter(virtualBotFileName, true, Encoding.UTF8);
        realBotTw = new StreamWriter(realBotFileName, true, Encoding.UTF8);
        kalmanBotTw = new StreamWriter(kalmanBotFileName, true, Encoding.UTF8);


        virtualBotTw.WriteLine((Time.time - virtualBotFirstTime).ToString("F3") + ";" + (virtualrobotController.posX + 0.15).ToString("F3") + ";" + virtualrobotController.posY.ToString("F3")
            + ";" + virtualBot.gameObject.transform.eulerAngles.y.ToString("F3") + ";" + virtualrobotController.forwardSpeed.ToString("F3"));

        if (udpServer.realBotValues.Length > 0) {
        realBotTw.WriteLine((udpServer.realBotValues[0] - realBotFirstTime).ToString("F3") +";" + (Time.time - kalmanBotFirstTime) + ";" + (robotController.distanceX + 0.15).ToString("F3") + ";" + robotController.distanceY.ToString("F3")
           + ";" + robotController.robot.gameObject.transform.eulerAngles.y.ToString("F3") + ";" + udpServer.realBotValues[1] + ";"+ udpServer.realBotValues[2]);
        }

        kalmanBotTw.WriteLine((Time.time - kalmanBotFirstTime).ToString("F3")+ ";" + (kalmanFusion.eK[0]).ToString("F3") + ";" + kalmanFusion.eOdon[0].ToString("F3") + ";" + kalmanFusion.eCam[0].ToString("F3") + ";" + kalmanFusion.yK[0].ToString("F3"));

        virtualBotTw.Close();
        realBotTw.Close();
        kalmanBotTw.Close();
    }
}
