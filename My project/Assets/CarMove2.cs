using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CarMove2 : MonoBehaviour
{
    public WheelCollider[] wheels = new WheelCollider[4];
    public Transform[] tires = new Transform[4];
    public float mortor = 50000.0f; //add motor power
    public float power = 3000f; //add car body power
    public float rot = 45f; //curve angle
    public float carSpeed = 40f; //initial car speed
    public bool carAtFoward = false;
    public bool isAvoidance = false;
    GameObject car = null;
    GameObject stopLine = null;
    Rigidbody rb;
    private Vector3 prePosition;
    private float curCarSpeed = 0f;
    //float delay = 5f;
    void Start()
    {
        car = GameObject.Find("car 1203 blue");
        stopLine = GameObject.Find("Plane2");
        rb = GetComponent<Rigidbody>();
        for (int i = 0; i < 4; i++)
        {
            wheels[i].steerAngle = 0;
            wheels[i].ConfigureVehicleSubsteps(5, 12, 13);
        }
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        prePosition = car.transform.position;
    }

    private void Update()
    {
        UpdateMeshesPostion();
    }

    void FixedUpdate()
    {
        GetCarSpeed(car);
        if (CarCam2.detectedHuman)
        {
            CarStop();
        }
        else
        {
            if (car.transform.position.z > (stopLine.transform.position.z + 25f))
            {
                if (CarCam2.detectedRed)
                {
                    CarStop();
                }
                else if (CarCam2.detectedYellow)
                {
                    //CarStop();
                }
                else if (CarCam2.detectedGreen)
                {
                    carSpeed = 40f;
                    CarForward(carSpeed);
                }
                else
                {
                    carSpeed = 40f;
                    CarForward(carSpeed);
                }
            }
            else if (car.transform.position.z < 50)
            {
                CarStop();
            }
            else
            {
                CarForward(carSpeed);
            }
        }
        


        if (carAtFoward)
        {
            rb.AddForce(transform.rotation * new Vector3(0, 0, power));
        }
        else
        {
            rb.AddForce(transform.rotation * new Vector3(0, 0, -power));
        }
    }

    void UpdateMeshesPostion()
    {
        for (int i = 0; i < 4; i++)
        {
            Quaternion quat;
            Vector3 pos;
            wheels[i].GetWorldPose(out pos, out quat);
            tires[i].position = pos;
            tires[i].rotation = quat;
        }
    }

    void GetCarSpeed(GameObject car)
    {
        Vector3 delta_Position = car.transform.position - prePosition;
        curCarSpeed = delta_Position.magnitude / Time.deltaTime;
        //Debug.Log("Speed: " + curCarSpeed);
        prePosition = transform.position;
    }

    void CarForward(float speed)
    {
        if (curCarSpeed < speed)
        {
            carAtFoward = true;
            for (int i = 0; i < 4; i++)
            {
                wheels[i].brakeTorque = 0;
                wheels[i].motorTorque = mortor * 2;
            }
        }
        else
        {
            carAtFoward = false;
            for (int i = 0; i < 4; i++)
            {
                wheels[i].brakeTorque = 0;
                wheels[i].motorTorque = 0;
            }
        }
    }

    void CarBack(float speed)
    {
        if (curCarSpeed < speed)
        {
            carAtFoward = false;
            for (int i = 0; i < 4; i++)
            {
                wheels[i].brakeTorque = 0;
                wheels[i].motorTorque = -(mortor * 2);
            }
        }
        else
        {
            carAtFoward = true;
            for (int i = 0; i < 4; i++)
            {
                wheels[i].brakeTorque = 0;
                wheels[i].motorTorque = 0;
            }
        }
    }

    void CarStop()
    {
        if (curCarSpeed > 5f)
        {
            for (int i = 0; i < 4; i++)
            {
                wheels[i].brakeTorque = mortor * 500;
                wheels[i].motorTorque = 0;
            }
            if (carAtFoward)
            {
                carAtFoward = false;
            }
            /*
            else
            {
                carAtFoward = true;
            }*/
        }
    }

    IEnumerator CarTurnLeft()
    {
        carSpeed = 20f;
        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = -45;
        }
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = 0;
        }
        yield return null;
    }

    IEnumerator CarTurnRight()
    {
        carSpeed = 20f;
        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = 45;
        }
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = 0;
        }
        yield return null;
    }

    IEnumerator Avoidance()
    {
        Debug.Log("turn left");
        yield return StartCoroutine("CarTurnLeft");

        Debug.Log("straight");
        carSpeed = 40f;
        yield return new WaitForSeconds(7f);

        Debug.Log("turn right");
        yield return StartCoroutine("CarTurnRight");

        Debug.Log("straight");
        carSpeed = 40f;
        yield return new WaitForSeconds(5f);

        isAvoidance = false;
    }

}