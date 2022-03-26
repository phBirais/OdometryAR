using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    public Text sliderRigthValue;
    public Text sliderLeftValue;
    public Text distanceValueX, distanceValueY, distanceDiffValueY;
    public Slider rightSlider;
    public Slider leftSlider;

    

    public float rightSliderValue;

    public float leftSliderValue;

    // Start is called before the first frame update

    VirtualBotController virtualrobotController;
    RobotController robotController;
    void Start()
    {
        robotController = GameObject.Find("RobotBody").GetComponent<RobotController>();
        virtualrobotController = GameObject.Find("VirtualRobot").GetComponent<VirtualBotController>();   
    }

    // Update is called once per frame
    void Update()
    {
        var virtualBotDist = Mathf.Round(((float)(robotController.distance)) * 100.0f) * 0.01f;
        var realBotDist = Mathf.Round(((float)(virtualrobotController.distance)) * 100.0f) * 0.01f;
        distanceValueX.text = virtualBotDist.ToString();
        distanceValueY.text = realBotDist.ToString();
        distanceDiffValueY.text = (virtualBotDist - realBotDist).ToString();
    }

     public void RightSliderValueChanged(float newValue)
 {
     rightSliderValue = Mathf.Round(rightSlider.value * 100) / 100;

     sliderRigthValue.text = rightSliderValue.ToString();

 }

      public void LeftSliderValueChanged(float newValue)
 {
     leftSliderValue = Mathf.Round(leftSlider.value * 100) / 100;

     sliderLeftValue.text = leftSliderValue.ToString();

 }

}
