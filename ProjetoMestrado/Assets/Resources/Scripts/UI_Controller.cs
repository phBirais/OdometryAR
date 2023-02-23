using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
   
    public Text realdistanceValueX, realdistanceValueY, distanceValueEncod, distanceDiffValueY;
    public Text virtualdistanceValueX, virtualdistanceValueY;
    public Text kalmandistanceValueX, kalmandistanceValueY;

    //private Text sliderRigthValue;
    //private Text sliderLeftValue;
    //public Slider rightSlider;
    //public Slider leftSlider;
    //public float rightSliderValue;
    //public float leftSliderValue;

    public VirtualBotSpeedController virtualBotSpeedController;
    public RobotController robotController;
    public UDPServer udpServer;
    public KalmanFusion kalmanFusion;

    void Start()
    {
        //robotController = GameObject.Find("RobotBody").GetComponent<RobotController>();
        //virtualrobotController = GameObject.Find("VirtualRobot").GetComponent<VirtualBotSpeedController>();   
    }

    void Update()
    {
        //robo virtrual
        var virtualBotDistX = virtualBotSpeedController.posX + 0.15;
        var virtualBotDistY = virtualBotSpeedController.posY;

        //robo real
        var realBotDistX = robotController.distanceX+0.15;
        var realBotDistY = robotController.distanceY;


        //robo filtro de kalman
        var kalmanBotDistX = kalmanFusion.eK[0] + 0.15;
        var kalmanBotDistY = kalmanFusion.eK[1];
        

        //Incluindo valores nas caixas de texto

        //valor Robo real
        realdistanceValueX.text = realBotDistX.ToString("F3");
        realdistanceValueY.text = realBotDistY.ToString("F3");

        //valor robo virtual
        virtualdistanceValueX.text = virtualBotDistX.ToString("F3");
        virtualdistanceValueY.text = virtualBotDistY.ToString("F3");

        //valor ponto filtro de kalman
        kalmandistanceValueX.text = kalmanBotDistX.ToString("F3");
        kalmandistanceValueY.text = kalmanBotDistY.ToString("F3");

        //valor de distancia percorrida pelos encoders
        distanceValueEncod.text = udpServer.distance.ToString("F3");

        //valor da diferença entre os dois (real e virtual)
        //distanceDiffValueY.text = (virtualBotDist - realBotDist).ToString();
    }

/*
 public void RightSliderValueChanged(float newValue)
 {
    rightSliderValue = Mathf.Round(rightSlider.value * 100) / 100;
    sliderRigthValue.text = rightSliderValue.ToString();
 }*/

/*
 public void LeftSliderValueChanged(float newValue)
 {
   leftSliderValue = Mathf.Round(leftSlider.value * 100) / 100;
   sliderLeftValue.text = leftSliderValue.ToString();
 }*/

}
