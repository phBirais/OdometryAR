using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualBotSpeedController : MonoBehaviour
{
    public float wheelRadius = 0.0165f; //Raio da roda em metros
    public float bodyRadius = 0.05f; //Raio do corpo do robô em metros
    public float leftWheelSpeed = 0.0f;
    public float rightWheelSpeed = 0.0f;
    public GameObject go;
    public GameObject baseMarker;

    public float deltaT = 1f; //passo da operação

    float cos_theta = 0, sin_theta = 0;

    float theta = 0;

    float xAtual, xAnterior = -0.15f, yAtual, yAnterior, anguloAtual, anguloAnterior = 0;


    public double posX, posY;

    void Start()
    {
         go.transform.position =  new Vector3(-0.15f, 0, 0);
         xAtual = go.transform.position.x;
    }
    public void StartRobot()
    {
        InvokeRepeating("Main", 0.1f, deltaT);
    }

    public void StopRobot()
    {
        CancelInvoke();
    }

    public void ResetRobot()
    {
        go.transform.position = new Vector3(-0.15f, 0, 0);
        go.transform.Rotate(0, 180, 0);
        xAnterior = -0.15f;
        yAnterior = 0;
        anguloAnterior = 0;
    }

    void Update()
    {
        Vector3 difference = (go.transform.position - baseMarker.transform.position);
        float distanceX =difference.x;
        float distanceZ = difference.z;

        //distanceX = Mathf.Round(distanceX * 100) / 100;
        //distanceZ = Mathf.Round(distanceZ * 100) / 100;

        //print(distanceX);
        posX = distanceX;
        posY = distanceZ;
    }

    // Update is called once per frame
    void Main()
    {
        float v = -1 * FowardSpeed();
        float omega = GetOmega();
        xAtual = xAnterior + (v * cos_theta * deltaT);
        yAtual = yAnterior + (v * sin_theta * deltaT);
        cos_theta = Mathf.Cos(theta);
        sin_theta = Mathf.Sin(theta);

        //Debug.Log("Theta: " + theta);
        //Debug.Log("CosTheta: " + cos_theta);
        //Debug.Log("SinTheta: " + sin_theta);
        //Debug.Log("V: " + v);
        
        //Debug.Log("xAtual: " + xAtual);
        //Debug.Log("yAtual: " + yAtual);

        xAnterior = xAtual;
        yAnterior = yAtual;
        anguloAnterior = theta;


        //Debug.Log("anguloAnterior: " + (anguloAnterior * Mathf.Rad2Deg));
        //movimentacao
        go.transform.position = new Vector3(xAtual, 0, yAtual);
        go.transform.Rotate(0, (-(omega * deltaT) * Mathf.Rad2Deg), 0);

        theta = anguloAnterior + (omega * deltaT);
    }

    float FowardSpeed()
    {
        float speed = 0;
        speed = ((leftWheelSpeed * wheelRadius) / 2) + ((rightWheelSpeed * wheelRadius) / 2);
        return speed;
    }

    float GetOmega()
    {
        float t = ((leftWheelSpeed * wheelRadius) - (rightWheelSpeed * wheelRadius)) / (2 * bodyRadius);

        return t;
    }
}
