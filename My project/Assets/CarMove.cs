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
    GameObject car = null;
    Rigidbody rb;
    private Vector3 prePosition;
    private float carSpeed = 0f;
    //float delay = 5f;
    void Start()
    {
        car = GameObject.Find("car 1203 yellow");
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
        if (car.transform.position.x < 150 && ObjectControl.isGreenLight)
        {
            StartCoroutine("CarTurnLeft");
        }

    }
    
    void FixedUpdate()
    {
        carSpeed = GetCarSpeed(car);
        //CarForward(30f);
        if (car.transform.position.x > 150)
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
        else if (car.transform.position.z < 400)
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
        Debug.Log("Speed: " + carSpeed);
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
        StopCoroutine("carTurnLeft");
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
        StopCoroutine("carTurnRight");
    }

    IEnumerator Avoidance()
    {
        carAtFoward = true;
        for (int i = 0; i < 4; i++)
        {
            wheels[i].brakeTorque = mortor*500;
            wheels[i].motorTorque = 0;
        }
        yield return new WaitForSeconds(2.0f);
        carAtFoward = false;
        for (int i = 0; i < 4; i++)
        {
            wheels[i].brakeTorque = 0;
        }
        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = -rot;
        }
        wheels[1].motorTorque = mortor;
        wheels[3].motorTorque = mortor;
        yield return new WaitForSeconds(4.0f);
        
        wheels[1].motorTorque = mortor;
        wheels[3].motorTorque = mortor;
        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = 0;
        }
        carAtFoward = true;
        for (int i = 0; i < 4; i++)
        {
            wheels[i].brakeTorque = mortor * 500;
            wheels[i].motorTorque = 0;
        }
        yield return new WaitForSeconds(1.5f);

        carAtFoward = false;
        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = rot;
        }
        wheels[0].motorTorque = mortor;
        wheels[2].motorTorque = mortor;
        //wheels[1].motorTorque = mortor;
        //wheels[3].motorTorque = mortor;

        yield return new WaitForSeconds(8f);

        carAtFoward = true;
        for (int i = 0; i < 4; i++)
        {
            wheels[i].brakeTorque = mortor * 500;
            wheels[i].motorTorque = 0;
        }
        yield return new WaitForSeconds(2.0f);

        carAtFoward = false;
        wheels[0].motorTorque = mortor;
        wheels[2].motorTorque = mortor;
        //wheels[1].motorTorque = mortor;
        //wheels[3].motorTorque = mortor;

        yield return new WaitForSeconds(8.0f);


        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = -rot;
        }
        wheels[1].motorTorque = mortor;
        wheels[3].motorTorque = mortor;
        yield return new WaitForSeconds(3.0f);

        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = 0;
        }
        yield return new WaitForSeconds(7.0f);
        StopCoroutine("Avoidance");
        StartCoroutine("MoveCar");
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