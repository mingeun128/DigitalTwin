using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CarMove : MonoBehaviour
{
    public WheelCollider[] wheels = new WheelCollider[4];
    public Transform[] tires = new Transform[4]; 
    public float maxF = 500.0f;
    public float power = 30000.0f;
    public float rot = 45;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        for (int i = 0; i < 4; i++)
        {
            wheels[i].steerAngle = 0;
            wheels[i].ConfigureVehicleSubsteps(5, 12, 13);
            Debug.Log(tires[i].transform.position);
        }
        rb.centerOfMass = new Vector3(0, 0, 0); //무게중심을 가운데로 맞춰서 안정적으로 주행하도록 한다.
    }

    private void Update()
    {
        UpdateMeshesPostion(); //바퀴가 돌아가는게 보이도록 함
    }

    void FixedUpdate()
    {
        float a = Input.GetAxis("Vertical");
        rb.AddForce(transform.rotation * new Vector3(0, 0, a * power)); //뒤에서 밀어준다.
        for (int i = 0; i < 4; i++)
        {
            wheels[i].motorTorque = maxF * a; //바퀴를 돌린다.
        }
        
        float steer = rot * Input.GetAxis("Horizontal");

        for (int i = 0; i < 2; i++) //앞바퀴만 회전한다.
        {
            wheels[i].steerAngle = steer; //여기도 바퀴와 콜라이더가 직각인사람은 + 90을 해줘야한다.
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
}