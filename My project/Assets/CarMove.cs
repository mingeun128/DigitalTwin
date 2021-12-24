using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CarMove : MonoBehaviour
{
    public WheelCollider[] wheels = new WheelCollider[4];
    public Transform[] tires = new Transform[4]; 
    public float mortor = 500000000.0f;
    public float power = 30000.0f;
    public float rot = 50;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        for (int i = 0; i < 4; i++)
        {
            wheels[i].steerAngle = 0;
            //wheels[i].ConfigureVehicleSubsteps(5, 12, 13);
        }
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        //Invoke("Avoidance", 8f);
    }

    private void Update()
    {
        UpdateMeshesPostion();
    }

    void FixedUpdate()
    {
        //float a = Input.GetAxis("Vertical");
        //rb.AddForce(transform.rotation * new Vector3(0, 0, power));
        if ()
        {

        }
        for (int i = 0; i < 4; i++)
        {
            wheels[i].motorTorque = mortor;
        }
        /*
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

    void Avoidance()
    {
        float delay = 1f;
        while (delay > 0f)
        {
            for (int i = 0; i < 4; i++)
            {
                wheels[i].motorTorque = -mortor;
            }
            delay -= Time.deltaTime;
        }

        delay = 5f;
        while (delay > 0f)
        {
            for (int i = 0; i < 2; i++)
            {
                wheels[i].steerAngle = rot;
            }
            wheels[1].motorTorque = mortor * 2;
            wheels[3].motorTorque = mortor * 2;
            delay -= Time.deltaTime;
        }
        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = 0;
        }

        delay = 3f;
        while (delay > 0f)
        {
            for (int i = 0; i < 2; i++)
            {
                wheels[i].steerAngle = -rot;
            }
            wheels[0].motorTorque = mortor * 2;
            wheels[2].motorTorque = mortor * 2;
            delay -= Time.deltaTime;
        }
    }
}