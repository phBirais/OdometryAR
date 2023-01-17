using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    //private Text sliderRigthValue;
    //private Text sliderLeftValue;
    public Text realdistanceValueX, realdistanceValueY, distanceValueEncod, distanceDiffValueY;
    public Text virtualdistanceValueX, virtualdistanceValueY;
    //public Slider rightSlider;
    //public Slider leftSlider;



    //public float rightSliderValue;

    //public float leftSliderValue;

    // Start is called before the first frame update

    public VirtualBotSpeedController virtualBotSpeedController;
    public RobotController robotController;
    public UDPServer udpServer;
    void Start()
    {
        //robotController = GameObject.Find("RobotBody").GetComponent<RobotController>();
        //virtualrobotController = GameObject.Find("VirtualRobot").GetComponent<VirtualBotSpeedController>();   
    }

    // Update is called once per frame
    void Update()
    {
        var virtualBotDistX = virtualBotSpeedController.posX + 0.15;
        var virtualBotDistY = virtualBotSpeedController.posY;
        var realBotDistX = robotController.distanceX+0.15;
        var realBotDistY = robotController.distanceY;
        realdistanceValueX.text = realBotDistX.ToString("F3");
        realdistanceValueY.text = realBotDistY.ToString("F3");
        virtualdistanceValueX.text = virtualBotDistX.ToString("F3");
        virtualdistanceValueY.text = virtualBotDistY.ToString("F3"); 
        distanceValueEncod.text = udpServer.distance.ToString("F3");
        //distanceDiffValueY.text = (virtualBotDist - realBotDist).ToString();
    }

 //    public void RightSliderValueChanged(float newValue)
 //{
 //    rightSliderValue = Mathf.Round(rightSlider.value * 100) / 100;

 //    sliderRigthValue.text = rightSliderValue.ToString();

 //}

 //     public void LeftSliderValueChanged(float newValue)
 //{
 //    leftSliderValue = Mathf.Round(leftSlider.value * 100) / 100;

 //    sliderLeftValue.text = leftSliderValue.ToString();

 //}

}
