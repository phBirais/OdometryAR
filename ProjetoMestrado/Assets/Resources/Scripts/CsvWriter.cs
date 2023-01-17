using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class CsvWriter : MonoBehaviour
{
    //referencias a outros scripts;
    public VirtualBotSpeedController virtualrobotController;
    public RobotController robotController;
    public UDPServer udpServer;
    public GameObject virtualBot;

    //tempo de start
    float virtualBotFirstTime = 0, realBotFirstTime = 0;

    string virtualBotFileName = "", realBotFileName = "";

    StreamWriter virtualBotTw, realBotTw;
    void Start()
    {
        //criação do arquivo para dados do robô virtual e real
        virtualBotFileName = Application.dataPath + "/virtualBotData.csv";
        realBotFileName = Application.dataPath + "/realBotData.csv";

        virtualBotTw = new StreamWriter(virtualBotFileName, false);
        virtualBotTw.WriteLine("Time; Xposition; Yposition; Angle; Speed");
        virtualBotTw.Close();

        realBotTw = new StreamWriter(realBotFileName, false);
        realBotTw.WriteLine("Time; Xposition; Yposition; Angle; RightEncoder; LeftEncoder");
        realBotTw.Close();


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
            if(udpServer.realBotValues.Length > 0)
                realBotFirstTime = udpServer.realBotValues[0];
        }
    }

    void WriteCSV()
    {
        //print("entrou");
        virtualBotTw = new StreamWriter(virtualBotFileName, true, Encoding.UTF8);
        realBotTw = new StreamWriter(realBotFileName, true, Encoding.UTF8);


        virtualBotTw.WriteLine((Time.time - virtualBotFirstTime).ToString("F3") + ";" + (virtualrobotController.posX + 0.15).ToString("F3") + ";" + virtualrobotController.posY.ToString("F3")
            + ";" + virtualBot.gameObject.transform.eulerAngles.y.ToString("F3") + ";" + virtualrobotController.forwardSpeed.ToString("F3"));

        if (udpServer.realBotValues.Length > 0) { 
        realBotTw.WriteLine((udpServer.realBotValues[0] - virtualBotFirstTime).ToString("F3") + ";" + (robotController.distanceX + 0.15).ToString("F3") + ";" + robotController.distanceY.ToString("F3")
           + ";" + robotController.robot.gameObject.transform.eulerAngles.y.ToString("F3") + ";" + virtualrobotController.forwardSpeed.ToString("F3"));
        }

        virtualBotTw.Close();
        realBotTw.Close();
    }
}
