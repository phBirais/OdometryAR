using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class SimulatedCSVbuilder : MonoBehaviour
{
    // Start is called before the first frame update
    public SpeedControl speedControl;
    public SimulatedKalman simulatedKalman;

    //tempo de start
    float botFirstMeasureTime = 0;
    string fileName = "";

    StreamWriter simulatedTw;
    void Start()
    {
        fileName = Application.dataPath + "/simulatedBotsData.csv";

        simulatedTw = new StreamWriter(fileName, false);
        simulatedTw.WriteLine("Time; Virtual x;Virtual y; Virtual angle; Eodom x; Eodom y; Eodom angle;" +
            " Ecam x; Ecam y; Ecam angle; Ek x; Ek y ; Ek angle; Ykx; Yky; Yk angle");
        simulatedTw.Close();

    }

    // Update is called once per frame
    void Update()
    {
        WriteCSV();
    }

    void WriteCSV()
    {
        simulatedTw = new StreamWriter(fileName, true, Encoding.UTF8);

        simulatedTw.WriteLine((Time.time - botFirstMeasureTime).ToString("F3") + ";" + (speedControl.posX).ToString("F3") + ";" + (speedControl.posY).ToString("F3") + ";" + (speedControl.angle).ToString("F3")
            + ";" + simulatedKalman.eOdon[0].ToString("F3") + ";" + simulatedKalman.eOdon[1].ToString("F3") + ";" + simulatedKalman.eOdon[2].ToString("F3")
            + ";" + simulatedKalman.eCam[0].ToString("F3") + ";" + simulatedKalman.eCam[1].ToString("F3") + ";" + simulatedKalman.eCam[2].ToString("F3")
            + ";" + simulatedKalman.eK[0].ToString("F3") + ";" + simulatedKalman.eK[1].ToString("F3") + ";" + simulatedKalman.eK[2].ToString("F3")
            + ";" + simulatedKalman.yK[0].ToString("F3") + ";" + simulatedKalman.yK[1].ToString("F3") + ";" + simulatedKalman.yK[2].ToString("F3"));

        simulatedTw.Close();
    }
}
