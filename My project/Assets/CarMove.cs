using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CarMove : MonoBehaviour
{
    public WheelCollider[] wheels = new WheelCollider[4];
    public Transform[] tires = new Transform[4]; 
    public float mortor = 5000000.0f;
    public float power = 100000f;
    public float rot = 50;
    public float obstaclePosition = 230;
    public bool carAtFoward = false;
    public bool isAvoidance = false;
    GameObject car = null;
    GameObject fence = null;
    GameObject stopLine = null;
    Rigidbody rb;
    private Vector3 prePosition;
    private float carSpeed = 0f;
    float delay = 0f;
    void Start()
    {
        car = GameObject.Find("car 1203 yellow");
        fence = GameObject.Find("Track_Fence");
        stopLine = GameObject.Find("Plane");
        rb = GetComponent<Rigidbody>();
        for (int i = 0; i < 4; i++)
        {
            wheels[i].steerAngle = 0;
            wheels[i].ConfigureVehicleSubsteps(5, 12, 13);
        }
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        //StartCoroutine("MoveCar");
        prePosition = car.transform.position;
        
    }

    private void Update()
    {
        UpdateMeshesPostion();

        if ((car.transform.position.x < (fence.transform.position.x + 450f)))
        {
            delay += Time.deltaTime;
            StartCoroutine("Avoidance", delay);
        }

    }
    
    void FixedUpdate()
    {
        carSpeed = GetCarSpeed(car);
        //CarForward(30f);
        if (car.transform.position.x > (stopLine.transform.position.x + 25f))
        {
            if (ObjectControl.isRedLight)
            {
                CarStop();
            }
            else if (ObjectControl.isYellowLight)
            {
                //CarStop();
            }
            else if (ObjectControl.isGreenLight)
            {
                CarForward(40f);
                //Debug.Log("GO GO");
            }
        }
        else if (car.transform.position.x < -800)
        {
            CarStop();
        }
        else
        {
            CarForward(40f);
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

    float GetCarSpeed(GameObject car)
    {
        Vector3 delta_Position = car.transform.position - prePosition;
        carSpeed = delta_Position.magnitude / Time.deltaTime;
        //Debug.Log("Speed: " + carSpeed);
        prePosition = transform.position;
        return carSpeed;
    }

    void CarForward(float speed)
    {
        if(carSpeed < speed)
        {
            carAtFoward = true;
            for (int i = 0; i < 4; i++)
            {
                wheels[i].motorTorque = mortor*2;
            }
        }
        else
        {
            carAtFoward = false;
            for (int i = 0; i < 4; i++)
            {
                wheels[i].motorTorque = 0;
            }
        }
    }

    void CarBack(float speed)
    {
        if (carSpeed < speed)
        {
            carAtFoward = false;
            for (int i = 0; i < 4; i++)
            {
                wheels[i].motorTorque = -(mortor * 2);
            }
        }
        else
        {
            carAtFoward = true;
            for (int i = 0; i < 4; i++)
            {
                wheels[i].motorTorque = 0;
            }
        }
    }

    void CarStop()
    {
        if (carSpeed > 5f)
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
            else
            {
                carAtFoward = true;
            }
        }
    }

    IEnumerator CarTurnLeft()
    {
        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = -45;
        }
        yield return new WaitForSeconds(20f);
        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = 0;
        }
        yield return null;
    }

    IEnumerator CarTurnRight()
    {
        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = 45;
        }
        yield return new WaitForSeconds(20f);
        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = 0;
        }
        yield return null;
    }

    IEnumerator Avoidance(float time)
    {
        if (time < 10f)
        {
            Debug.Log("turn left");
            yield return StartCoroutine("CarTurnLeft");
        }
        else if (time < 15f)
        {
            Debug.Log("straight");
            yield return new WaitForSeconds(1.5f);
        }
        else if (time < 25f)
        {
            Debug.Log("turn right");
            yield return StartCoroutine("CarTurnRight");
        }
        

        

        //yield return StartCoroutine("CarTurnLeft");

        //yield return StartCoroutine("CarTurnRight");
    }

    IEnumerator MoveCar()
    {
        while (car.transform.position.x > obstaclePosition + 400)
        {
            carAtFoward = false;
            for (int i = 0; i < 4; i++)
            {
                wheels[i].motorTorque = mortor;
            }
            yield return new WaitForSeconds(0.2f);
        }
        if ((car.transform.position.x <= obstaclePosition + 400) && (car.transform.position.x > obstaclePosition + 300))
        {
            StopCoroutine("MoveCar");
            StartCoroutine("Avoidance");
        }
        if (car.transform.position.x < 100)
        {
            Debug.Log("BRAKE");
            carAtFoward = true;
            for (int i = 0; i < 4; i++)
            {
                wheels[i].brakeTorque = mortor * 500;
                wheels[i].motorTorque = 0;
            }
            yield return new WaitForSeconds(2.0f);
        }

    }
}