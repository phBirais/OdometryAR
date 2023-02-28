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
        simulatedTw.WriteLine("Time; Virtual X position; Eodom x position; Ecam x position; Ek x position; Yk");
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

        simulatedTw.WriteLine((Time.time - botFirstMeasureTime).ToString("F3") + ";" + (speedControl.posX).ToString("F3") + ";" + simulatedKalman.eOdon[0].ToString("F3")
            + ";" + simulatedKalman.eCam[0].ToString("F3") + ";" + simulatedKalman.eK[0].ToString("F3") + ";" + simulatedKalman.yK[0].ToString("F3"));

        simulatedTw.Close();
    }
}
