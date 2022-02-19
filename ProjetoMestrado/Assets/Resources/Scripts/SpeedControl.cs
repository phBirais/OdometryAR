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

    float theta = -180;

    float xAtual, xAnterior, yAtual, yAnterior, anguloAtual, anguloAnterior=0 ;
    void Start()
    {
        InvokeRepeating("Main", 0.0f, deltaT);
    }

    // Update is called once per frame
    void Main()
    {
        float v = -1*FowardSpeed(); 
        float omega = GetOmega();        
        xAtual = xAnterior + (v *  cos_theta * deltaT);
        yAtual = yAnterior + (v *  sin_theta * deltaT);
        cos_theta = Mathf.Cos(theta);
        sin_theta = Mathf.Sin(theta);        
        
        Debug.Log("Theta: "+ theta);
        Debug.Log("CosTheta: "+cos_theta);
        Debug.Log("SinTheta: "+sin_theta);
        Debug.Log("V: "+v);
        Debug.Log("xAtual: "+xAtual);
        Debug.Log("yAtual: "+yAtual);

        xAnterior = xAtual;
        yAnterior= yAtual;
        anguloAnterior = theta;


        Debug.Log("anguloAnterior: " + (anguloAnterior * Mathf.Rad2Deg));
        //movimentacao
        go.transform.position = new Vector3(xAtual, 0, yAtual);
        go.transform.Rotate(0,(-(omega*deltaT)*Mathf.Rad2Deg),0);

        theta = anguloAnterior+ (omega * deltaT);
    }

    float FowardSpeed(){
        float speed = 0;
        speed = ((leftWheelSpeed*wheelRadius)/2) + ((rightWheelSpeed*wheelRadius)/2); 
        return speed;
    }

    float GetOmega(){
      float t = ((leftWheelSpeed*wheelRadius)-(rightWheelSpeed*wheelRadius))/(2*bodyRadius);

        return t;
    }
}
