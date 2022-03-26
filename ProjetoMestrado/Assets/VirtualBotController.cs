using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VirtualBotController : MonoBehaviour
{
    public GameObject marker;
    public GameObject robot;

    // Start is called before the first frame
    public float robotVelocity;
    public Vector3 robotAngle;
    public double distance, distanceY;

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
        robotVelocity = 1;
        //robotBody.velocity = new Vector3(robotVelocity, 0, 0);

    }
}
