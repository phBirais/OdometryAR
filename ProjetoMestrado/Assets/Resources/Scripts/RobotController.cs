using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public Rigidbody robotBody;

    public GameObject marker;
    public GameObject robot;

    public float robotVelocity;
    public Vector3 robotAngle;
    public double distance, distanceY;

    UI_Controller uiController;
    
    void Start()
    {
        // uiController = GameObject.Find("UIManager").GetComponent<UI_Controller>();        
    }

    // Update is called once per frame
    void Update()
    {
    
    Vector3 difference = (robot.transform.position - marker.transform.position);
    float distanceX = Mathf.Abs(difference.x);
    float distanceZ = Mathf.Abs(difference.z);
    
    //distance= Vector3.Distance (robot.transform.position, marker.transform.position);
    //distance = Mathf.Round(distance * 100) / 100;
    distanceX = Mathf.Round(distanceX * 100) / 100;
    distanceZ = Mathf.Round(distanceZ * 100) / 100;

    //print(distanceX);
    distance = distanceX - 0.15;
    distanceY = distanceZ;
    //robotVelocity = uiController.rightSliderValue;
    //robotVelocity = 1;
    //robotBody.velocity = new Vector3(robotVelocity,0,0);

    //mudar angulo do robô
    //Quaternion deltaRotation = Quaternion.Euler((new Vector3(0, robotAngle, 0))* Time.deltaTime);
    //robotAngle = new Vector3 (0, uiController.leftSliderValue, 0 );
    //robot.transform.Rotate(robotAngle * Time.deltaTime);
    //robotBody.MoveRotation(robotBody.rotation * deltaRotation);
    
    }
}
