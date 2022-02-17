using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCam2 : MonoBehaviour
{
    public static bool detectedGreen = false;
    public static bool detectedYellow = false;
    public static bool detectedRed = false;
    public static bool detectedHuman = false;

    GameObject trafficLight1 = null;
    GameObject trafficLight2 = null;
    GameObject human = null;

    private Transform cameraTr;

    private float recognitionRange = 150f;

    // Start is called before the first frame update
    void Start()
    {
        trafficLight1 = GameObject.Find("Traffic Light 1");
        trafficLight2 = GameObject.Find("Traffic Light 2");
        human = GameObject.Find("Human");
    }

    // Update is called once per frame
    void Update()
    {
        cameraTr = GetComponent<Transform>();
        //Debug.Log(cameraTr.eulerAngles.y);
        if (cameraTr.eulerAngles.y > 160 && cameraTr.eulerAngles.y < 200)
        {
            if (cameraTr.position.z < trafficLight2.transform.position.z + recognitionRange)
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
            if ((cameraTr.position.z - recognitionRange < human.transform.position.z) && (cameraTr.position.z > human.transform.position.z))
            {
                if ((cameraTr.position.x + 70f > human.transform.position.x) && (cameraTr.position.x - 70f < human.transform.position.x))
                {
                    Debug.Log("Blue car : DETECT HUMAN!!");
                    DetectedPedestrian();
                }
                else
                {
                    UnDetectedPedestrian();
                }
            }
            else
            {
                UnDetectedPedestrian();
            }
        }

        if (cameraTr.eulerAngles.y > 250 && cameraTr.eulerAngles.y < 290)
        {
            if (cameraTr.position.x < trafficLight1.transform.position.x + recognitionRange)
            {
                if (ObjectControl.isGreenLight1)
                {
                    DetectedGreen();
                    UnDetectedYellow();
                    UnDetectedRed();
                }
                if (ObjectControl.isYellowLight1)
                {
                    DetectedYellow();
                    UnDetectedGreen();
                    UnDetectedRed();
                }
                if (ObjectControl.isRedLight1)
                {
                    DetectedRed();
                    UnDetectedGreen();
                    UnDetectedYellow();
                }
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
    private void DetectedPedestrian()
    {
        detectedHuman = true;
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
    private void UnDetectedPedestrian()
    {
        detectedHuman = false;
    }
}
