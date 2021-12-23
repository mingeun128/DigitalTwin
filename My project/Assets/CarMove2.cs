using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove2 : MonoBehaviour
{
    // 휠콜라이더 4개
    public WheelCollider[] wheels = new WheelCollider[4];
    // 차량 모델의 바퀴 부분 4개
    GameObject[] wheelMesh = new GameObject[4];


    void Start()
    {
        // 바퀴 모델을 태그를 통해서 찾아온다.(차량이 변경되더라도 자동으로 찾기위해서)
        wheelMesh = GameObject.FindGameObjectsWithTag("WheelMesh");

        for (int i = 0; i < wheelMesh.Length; i++)
        {	// 휠콜라이더의 위치를 바퀴메쉬의 위치로 각각 이동시킨다.
            wheels[i].transform.position = wheelMesh[i].transform.position;
            Debug.Log(wheelMesh[i].transform.position);
        }
    }
    void WheelPosAndAni()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < 4; i++)
        {
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheelMesh[i].transform.position = wheelPosition;
            wheelMesh[i].transform.rotation = wheelRotation;
        }
    }
    void FixedUpdate()
    {
        WheelPosAndAni();
    }
}