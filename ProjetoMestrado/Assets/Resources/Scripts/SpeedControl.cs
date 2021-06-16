using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedControl : MonoBehaviour
{
    public float wheelRadius = 0.016f; //Raio da roda em metros
    public float bodyRadius = 0.05f; //Raio do corpo do robô em metros
    public float leftWheelSpeed = 0.0f;
    public float rightWheelSpeed = 0.0f;
    public GameObject go;

    public float deltaT = 1f; //passo da operação

    float cos_theta=0, sin_theta=0;

    float xAtual, xAnterior, yAtual, yAnterior, anguloAtual, anguloAnterior=0 ;
    void Start()
    {
        InvokeRepeating("Main", 0.0f, deltaT);
    }

    // Update is called once per frame
    void Main()
    {
        float v = FowardSpeed(); 
        float theta = GetTheta();
        cos_theta = Mathf.Cos(theta);
        sin_theta = Mathf.Sin(theta);
        xAtual = xAnterior + (v * deltaT * cos_theta);
        yAtual = yAnterior + (v * deltaT * sin_theta);
        anguloAtual = -theta *deltaT;
        /*
        Debug.Log("Theta: "+theta);
        Debug.Log("CosTheta: "+cos_theta);
        Debug.Log("SinTheta: "+sin_theta);
        Debug.Log("V: "+v);
        Debug.Log("xAtual: "+xAtual);
        Debug.Log("yAtual: "+yAtual);*/
        xAnterior = xAtual;
        yAnterior= yAtual;
        anguloAnterior  = anguloAtual;
        go.transform.position = new Vector3(xAtual, 0, yAtual);
        go.transform.Rotate(0,anguloAnterior,0);
    }

    float FowardSpeed(){
        float speed = 0;
        speed = ((leftWheelSpeed*wheelRadius)/2) + ((rightWheelSpeed*wheelRadius)/2); 
        return speed;
    }

    float GetTheta(){
      float t = ((leftWheelSpeed*wheelRadius)-(rightWheelSpeed*wheelRadius))/bodyRadius;

        return t;
    }
}
