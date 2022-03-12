using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCam : MonoBehaviour
{
    public static bool detectedGreen = false;
    public static bool detectedYellow = false;
    public static bool detectedRed = false;
    public static bool detectedObstacle = false;

    GameObject trafficLight1 = null;
    GameObject trafficLight2 = null;

    GameObject obstacle = null;

    private Transform cameraTr;

    private float recognitionRange = 150f;

    // Start is called before the first frame update
    void Start()
    {
        trafficLight1 = GameObject.Find("Traffic Light 1");
        trafficLight2 = GameObject.Find("Traffic Light 2");
        obstacle = GameObject.Find("Track_Fence");
    }

    // Update is called once per frame
    void Update()
    {
        cameraTr = GetComponent<Transform>();
        //Debug.Log(cameraTr.eulerAngles.y);
        if (cameraTr.eulerAngles.y > 160 && cameraTr.eulerAngles.y <200)
        {
            if ((cameraTr.position.z - recognitionRange < trafficLight2.transform.position.z) && (cameraTr.position.z > trafficLight2.transform.position.z))
            {
                if (ObjectControl.isGreenLight2)
                {
                    DetectedGreen();
                    UnDetectedYellow();
                    UnDetectedRed();
                }
                if (ObjectControl.isYellowLight2)
                {
                    DetectedYellow();
                    UnDetectedGreen();
                    UnDetectedRed();
                }
                if (ObjectControl.isRedLight2)
                {
                    DetectedRed();
                    UnDetectedGreen();
                    UnDetectedYellow();
                }
            }
        }

        if (cameraTr.eulerAngles.y > 250 && cameraTr.eulerAngles.y < 290)
        {
            if ((cameraTr.position.x - recognitionRange < trafficLight1.transform.position.x) && (cameraTr.position.x > trafficLight1.transform.position.x))
            {
                if (ObjectControl.isGreenLight1)
                {
                    //Debug.Log("Yellow car : DETECT GREEN!!");
                    DetectedGreen();
                    UnDetectedYellow();
                    UnDetectedRed();
                }
                if (ObjectControl.isYellowLight1)
                {
                    //Debug.Log("Yellow car : DETECT YELLOW!!");
                    DetectedYellow();
                    UnDetectedGreen();
                    UnDetectedRed();
                }
                if (ObjectControl.isRedLight1)
                {
                    //Debug.Log("Yellow car : DETECT RED!!");
                    DetectedRed();
                    UnDetectedGreen();
                    UnDetectedYellow();
                }
            }
            if ((cameraTr.position.x - 350f < obstacle.transform.position.x) && (cameraTr.position.x > obstacle.transform.position.x))
            {
                //Debug.Log("Yellow car : DETECT OBSTACLE !!!");
                DetectedObstacle();
            }
            else if (cameraTr.position.x < obstacle.transform.position.x)
            {
                UnDetectedObstacle();
            }
        }
    }

    private void DetectedGreen()
    {

        detectedGreen = true;
    }
    private void DetectedYellow()
    {
        detectedYellow = true;
    }
    private void DetectedRed()
    {
        detectedRed = true;
    }
    private void DetectedObstacle()
    {
        detectedObstacle = true;
    }

    private void UnDetectedGreen()
    {
        detectedGreen = false;
    }
    private void UnDetectedYellow()
    {
        detectedYellow = false;
    }
    private void UnDetectedRed()
    {
        detectedRed = false;
    }
    private void UnDetectedObstacle()
    {
        detectedObstacle = false;
    }
}
