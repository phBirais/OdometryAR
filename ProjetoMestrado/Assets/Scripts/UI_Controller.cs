using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    public Text sliderRigthValue;
    public Text sliderLeftValue;
    public Text distanceValueX, distanceValueY;
    public Slider rightSlider;
    public Slider leftSlider;

    

    public float rightSliderValue;

    public float leftSliderValue;

    // Start is called before the first frame update

    RobotController robotController;
    void Start()
    {
        robotController = GameObject.Find("RobotBody").GetComponent<RobotController>();   
    }

    // Update is called once per frame
    void Update()
    {
        distanceValueX.text = robotController.distance.ToString();
        distanceValueY.text = robotController.distanceY.ToString();
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
