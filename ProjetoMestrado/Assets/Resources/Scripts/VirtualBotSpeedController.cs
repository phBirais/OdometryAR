using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualBotSpeedController : MonoBehaviour
{
    public float wheelRadius = 0.0165f; //Raio da roda em metros
    public float bodyRadius = 0.05f; //Raio do corpo do robô em metros
    public float leftWheelSpeed = 0.0f;
    public float rightWheelSpeed = 0.0f;
    public GameObject go;
    public GameObject baseMarker;

    public InputField leftSpeedInput;
    public InputField rightSpeedInput;

    public float deltaT = 1f; //passo da operação

    float cos_theta = 0, sin_theta = 0;

    public float theta = 0;

    float xAtual, xAnterior = -0.15f, yAtual, yAnterior, anguloAtual, anguloAnterior = 0;

    bool status = false;

    Quaternion rotacaoInicial = Quaternion.identity;

    //variaveis publicas
    public double posX, posY, angleRadians;
    public float forwardSpeed;
    public float angularSpeed;

    float distanceX = 0;
    float distanceZ = 0;

    void Start()
    {
        go.transform.position =  new Vector3(-0.15f, 0, 0);
        xAtual = go.transform.position.x;       
    }
    public void StartRobot()
    {
        rotacaoInicial = go.transform.rotation;
        if (int.TryParse(leftSpeedInput.text, out int result))
        {
            rightWheelSpeed = result;
        }

        if (int.TryParse(rightSpeedInput.text, out int result2))
        {
            leftWheelSpeed = result2;
        }

        leftWheelSpeed = ((float)(leftWheelSpeed / 9.5492965964254));
        rightWheelSpeed = ((float)(rightWheelSpeed / 9.5492965964254));

        status = true;

        InvokeRepeating("Main", 0.1f, deltaT);
    }

    public void StopRobot()
    {
        //CancelInvoke();
        status = false;
    }

    public void ResetRobot()
    {
        go.transform.position = new Vector3(-0.15f, 0, 0);
        go.transform.rotation = rotacaoInicial;
        //go.transform.Rotate(0, -90, 0);
        xAnterior = -0.15f;
        yAnterior = 0;
        xAtual = 0;
        yAtual = 0;
        cos_theta = 0;
        sin_theta = 0;
        theta = 0;
        anguloAnterior = 0;
    }

    void FixedUpdate()
    {
        if (go.activeInHierarchy && baseMarker)
        {
            Vector3 difference = (go.transform.position - baseMarker.transform.position);
            distanceX = difference.x;
            distanceZ = difference.z;
        }

        //distanceX = Mathf.Round(distanceX * 100) / 100;
        //distanceZ = Mathf.Round(distanceZ * 100) / 100;

        //print(distanceX);
        posX = distanceX;
        posY = distanceZ;
    }

    // Update is called once per frame
    void Main()
    {
        if (status == true)
        {
            float v = -1 * FowardSpeed();
            float omega = GetOmega();
            xAtual = xAnterior + (v * cos_theta * deltaT);
            yAtual = yAnterior + (v * sin_theta * deltaT);
            cos_theta = Mathf.Cos(theta);
            sin_theta = Mathf.Sin(theta);

            xAnterior = xAtual;
            yAnterior = yAtual;
            anguloAnterior = theta;

            //movimentacao
            if (go.activeInHierarchy)
            {
                go.transform.position = new Vector3(xAtual, 0, yAtual);
                go.transform.Rotate(0, (-(omega * deltaT) * Mathf.Rad2Deg), 0);
            }

            theta = anguloAnterior + (omega * deltaT);
            forwardSpeed = v;
            angularSpeed = omega;
        }
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
