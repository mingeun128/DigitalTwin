using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    GameObject cube = null;
    GameObject sphere = null;
    GameObject jeep = null;
    int cubeTime = 0;
    int sphereTime = 0;
    int jeepTime = 0;
    float distance;
    float cubeSpeed;
    float sphereSpeed;
    float jeepSpeed;
    float time = 0;

    //Import C++ func
    [DllImport("Dll1")]
    public static extern int getCubeTime();
    [DllImport("Dll1")]
    public static extern int getSphereTime();
    [DllImport("Dll1")]
    public static extern int getJeepTime();
    [DllImport("Dll1")]
    public static extern int getDistance();

    // Start is called before the first frame update
    void Start()
    {
        //Set objects information
        distance = (float)getDistance();

        cube = GameObject.Find("Cube");
        cubeTime = getCubeTime();
        cubeSpeed = distance / cubeTime;

        sphere = GameObject.Find("Sphere");
        sphereTime = getSphereTime();
        sphereSpeed = distance / sphereTime;
        /*
        jeep = GameObject.Find("Jeep");
        jeepTime = getJeepTime();
        jeepSpeed = distance / jeepTime;*/
    }

    // Update is called once per frame
    void Update()
    {
        //Print current time
        time += Time.deltaTime;
        //Debug.Log("Time(sec): " + time);

        if (cube.transform.position.z < distance)
        {
            cube.transform.Translate(0, 0, cubeSpeed * Time.deltaTime);
        }
        if (sphere.transform.position.z < distance)
        {
            sphere.transform.Translate(0, 0, sphereSpeed * Time.deltaTime);
        }
        /*
        if (jeep.transform.position.z < distance)
        {
            jeep.transform.Translate(0, 0, jeepSpeed * Time.deltaTime);
        }
        */
        
    }
}