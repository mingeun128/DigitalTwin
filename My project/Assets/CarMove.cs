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
        rb.centerOfMass = new Vector3(0, 0, 0); //�����߽��� ����� ���缭 ���������� �����ϵ��� �Ѵ�.
    }

    private void Update()
    {
        UpdateMeshesPostion(); //������ ���ư��°� ���̵��� ��
    }

    void FixedUpdate()
    {
        float a = Input.GetAxis("Vertical");
        rb.AddForce(transform.rotation * new Vector3(0, 0, a * power)); //�ڿ��� �о��ش�.
        for (int i = 0; i < 4; i++)
        {
            wheels[i].motorTorque = maxF * a; //������ ������.
        }
        
        float steer = rot * Input.GetAxis("Horizontal");

        for (int i = 0; i < 2; i++) //�չ����� ȸ���Ѵ�.
        {
            wheels[i].steerAngle = steer; //���⵵ ������ �ݶ��̴��� �����λ���� + 90�� ������Ѵ�.
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