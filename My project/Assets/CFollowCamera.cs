using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFollowCamera : MonoBehaviour
{
    public Transform target;        // ����ٴ� Ÿ�� ������Ʈ�� Transform

    private Transform tr;                // ī�޶� �ڽ��� Transform

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void LateUpdate()
    {
        tr.position = new Vector3(target.position.x - 0.52f, tr.position.y, target.position.z - 6.56f);

        tr.LookAt(target);
    }
}
