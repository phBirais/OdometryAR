using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealBotOdometry : MonoBehaviour
{
    public UDPServer udpServer;
    public VirtualBotSpeedController virtualBotSpeedController;
    public GameObject realRobot;

    public float wheelRadius = 0.0165f; //Raio da roda em metros
    public float bodyRadius = 0.05f; //Raio do corpo do robô em metros

    float rightWheelSpeed = 0, leftWheelSpeed = 0;

    float cos_theta = 0, sin_theta = 0;
    float theta = 0;
    float xAtual, xAnterior = -0.15f, yAtual, yAnterior, anguloAtual, anguloAnterior = 0;

    //valores do encoder lido
    float encoderD=0, encoderE=0, encoderDAnt=0, encoderEAnt=0;
    float tempo=0, tempoAnt=0;
    int ppr = 440; //pulsos por rotação

    //vetor de estados odometris
    public Vector3 odometryState = Vector3.zero;

    //velocidade V
    public float forwardSpeed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartOdometryCalculus()
    {
        InvokeRepeating("CalculateRobotState", 0.1f, virtualBotSpeedController.deltaT);
    }

    public void StopOdometryCalculus()
    {
        CancelInvoke();
    }

    void CalculateRobotState()
    {
        if (udpServer.realBotValues.Length > 0)
        {
            float v = -1 * FowardSpeed(udpServer.realBotValues);
            float omega = GetOmega();

            xAtual = xAnterior + (v * cos_theta * virtualBotSpeedController.deltaT);
            yAtual = yAnterior + (v * sin_theta * virtualBotSpeedController.deltaT);
            cos_theta = Mathf.Cos(theta);
            sin_theta = Mathf.Sin(theta);

            //atualização dos valores Anteriores
            xAnterior = xAtual;
            yAnterior = yAtual;
            anguloAnterior = theta;

            odometryState = new Vector3(xAtual, yAtual, theta);

            theta = anguloAnterior + (-omega * virtualBotSpeedController.deltaT);
            forwardSpeed = v;
        }
    }

     float FowardSpeed(float[] encoderValues)
    {        
        tempo = Time.time;
        encoderD = encoderValues[1];
        encoderE = encoderValues[2];
        //Debug.Log("tempo:" + tempo);
        //Debug.Log("tempo dif:" + (tempo - tempoAnt));
        //Debug.Log("encoder:" + encoderD);
        if (encoderD - encoderDAnt > 0 && encoderE - encoderEAnt > 0)
        {
            //velocidade roda direita em rad/s
            rightWheelSpeed = ((encoderD - encoderDAnt) / (tempo - tempoAnt)); // diferença de pulsos / diferença de tempo
            rightWheelSpeed /= ppr; // divisao pelos pulsos por segundo para obter rotacoes por segundo
            rightWheelSpeed *= (2 * Mathf.PI); //convertendo para rad.s
           // Debug.Log("velocidade right  " + rightWheelSpeed);

            //velocidade roda esquerda em rad/s
            leftWheelSpeed = ((encoderE - encoderEAnt) / (tempo - tempoAnt));
            leftWheelSpeed /= ppr;
            leftWheelSpeed *= (2 * Mathf.PI);
           // Debug.Log("Velocidade esquerda  " + leftWheelSpeed);

            tempoAnt = tempo;
            encoderDAnt = encoderD;
            encoderEAnt = encoderE;
        }

        float speed = ((leftWheelSpeed * wheelRadius) / 2) + ((rightWheelSpeed * wheelRadius) / 2);            


        return speed;
    }

    float GetOmega()
    {
        float o = ((leftWheelSpeed * wheelRadius) - (rightWheelSpeed * wheelRadius)) / (2 * bodyRadius);
        return o;
    }
}
