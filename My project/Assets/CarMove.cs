using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CarMove : MonoBehaviour
{
    public WheelCollider[] wheels = new WheelCollider[4];
    public Transform[] tires = new Transform[4]; 
    public float mortor = 5000000.0f;
    public float power = 10000f;
    public float rot = 50;
    public float obstaclePosition = 230;
    public bool isbrake = false;
    GameObject car = null;
    Rigidbody rb;

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
        StartCoroutine("MoveCar");
        if (car.transform.position.x < 600)
        {
            StartCoroutine("Avoidance");
        }
        //Invoke("Avoidance", 7f);
    }

    private void Update()
    {
        UpdateMeshesPostion();
    }
    
    void FixedUpdate()
    {
        //float a = Input.GetAxis("Vertical");
        if (isbrake)
        {
            rb.AddForce(transform.rotation * new Vector3(0, 0, -power));
        }
        else
        {
            rb.AddForce(transform.rotation * new Vector3(0, 0, power));
        }
        
        /*
        if (car.transform.position.x < 600)
        {
            Avoidance();
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                wheels[i].motorTorque = mortor;
            }
        }
        
        float steer = rot * Input.GetAxis("Horizontal");

        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = steer;
        }
        */
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

    IEnumerator Avoidance()
    {
        isbrake = true;
        for (int i = 0; i < 4; i++)
        {
            wheels[i].brakeTorque = mortor*500;
            wheels[i].motorTorque = 0;
        }
        yield return new WaitForSeconds(2.0f);
        isbrake = false;
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
        isbrake = true;
        for (int i = 0; i < 4; i++)
        {
            wheels[i].brakeTorque = mortor * 500;
            wheels[i].motorTorque = 0;
        }
        yield return new WaitForSeconds(1.5f);

        isbrake = false;
        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = rot;
        }
        wheels[0].motorTorque = mortor;
        wheels[2].motorTorque = mortor;
        //wheels[1].motorTorque = mortor;
        //wheels[3].motorTorque = mortor;

        yield return new WaitForSeconds(8f);

        isbrake = true;
        for (int i = 0; i < 4; i++)
        {
            wheels[i].brakeTorque = mortor * 500;
            wheels[i].motorTorque = 0;
        }
        yield return new WaitForSeconds(2.0f);

        isbrake = false;
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
            isbrake = false;
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
            isbrake = true;
            for (int i = 0; i < 4; i++)
            {
                wheels[i].brakeTorque = mortor * 500;
                wheels[i].motorTorque = 0;
            }
            yield return new WaitForSeconds(2.0f);
        }

    }
}