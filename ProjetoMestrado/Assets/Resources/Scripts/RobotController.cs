using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public GameObject marker;
    public GameObject robot;

    //public float robotVelocity;
    public Vector3 robotAngle;
    public double distanceX, distanceY, rotation;
    
    void Start()
    {
        // uiController = GameObject.Find("UIManager").GetComponent<UI_Controller>();        
    }

    // Update is called once per frame
    void Update()
    {
    
    Vector3 positiondifference = (robot.transform.position - marker.transform.position);

        float rotationDifference = (robot.transform.rotation.y - marker.transform.rotation.y);
        float posX = positiondifference.x; //Mathf.Abs(difference.x);
        float posY = positiondifference.z;//Mathf.Abs(difference.z);

        //distance = Mathf.Round(distance * 100) / 100;
        distanceX = posX;//Mathf.Round(posX * 100) / 100;
        distanceY = posY;//Mathf.Round(posY * 100) / 100;
        rotation = rotationDifference;


    }
}
