using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public Rigidbody robotBody;

    public GameObject marker;
    public GameObject robot;

    //public float robotVelocity;
    public Vector3 robotAngle;
    public double distanceX, distanceY;

    UI_Controller uiController;
    
    void Start()
    {
        // uiController = GameObject.Find("UIManager").GetComponent<UI_Controller>();        
    }

    // Update is called once per frame
    void Update()
    {
    
    Vector3 difference = (robot.transform.position - marker.transform.position);
        //print(difference.ToString());
        float posX = difference.x; //Mathf.Abs(difference.x);
        float posY = difference.z;//Mathf.Abs(difference.z);

        //distance = Mathf.Round(distance * 100) / 100;
        distanceX = posX;//Mathf.Round(posX * 100) / 100;
        distanceY = posY;//Mathf.Round(posY * 100) / 100;
    
    }
}
