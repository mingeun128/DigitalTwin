using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    public static bool isRedLight = false;
    public static bool isYellowLight = false;
    public static bool isGreenLight = false;
    public GameObject lights = null;
    private Material red;
    private Material amber;
    private Material green;
    private Material redLight;
    private Material yellowLight;
    private Material greenLight;
    /*
    //Import C++ func
    [DllImport("Dll1")]
    public static extern int getCubeTime();
    [DllImport("Dll1")]
    public static extern int getSphereTime();
    [DllImport("Dll1")]
    public static extern int getJeepTime();
    [DllImport("Dll1")]
    public static extern int getDistance();
    */
    // Start is called before the first frame update
    void Start()
    {
        red = Resources.Load<Material>("Red");
        amber = Resources.Load<Material>("Amber");
        green = Resources.Load<Material>("Green");
        redLight = Resources.Load<Material>("RedLight");
        yellowLight = Resources.Load<Material>("YellowLight");
        greenLight = Resources.Load<Material>("GreenLight");
        lights = GameObject.Find("Lights");
        StartCoroutine("TrafficControl");
    }

    // Update is called once per frame
    void Update()
    {
        if (isRedLight == true)
        {
            TurnOnRed();
        }
        if (isYellowLight == true)
        {
            TurnOnYellow();
        }
        if (isGreenLight == true)
        {
            TurnOnGreen();
        }

    }

    private void TurnOnRed()
    {
        Material[] material = lights.GetComponent<MeshRenderer>().materials;
        material[0] = amber;
        material[1] = redLight;
        material[2] = green;
        lights.GetComponent<MeshRenderer>().materials = material;
    }
    private void TurnOnYellow()
    {
        Material[] material = lights.GetComponent<MeshRenderer>().materials;
        material[0] = yellowLight;
        material[1] = red;
        material[2] = green;
        lights.GetComponent<MeshRenderer>().materials = material;
    }
    private void TurnOnGreen()
    {
        Material[] material = lights.GetComponent<MeshRenderer>().materials;
        material[0] = amber;
        material[1] = red;
        material[2] = greenLight;
        lights.GetComponent<MeshRenderer>().materials = material;
    }

    IEnumerator TrafficControl()
    {
        while (true)
        {
            isRedLight = false;
            isYellowLight = false;
            isGreenLight = true;
            Debug.Log("Green");
            yield return new WaitForSeconds(8f);

            isRedLight = false;
            isYellowLight = true;
            isGreenLight = false;
            Debug.Log("Yellow");
            yield return new WaitForSeconds(1.5f);

            isRedLight = true;
            isYellowLight = false;
            isGreenLight = false;
            Debug.Log("Red");
            yield return new WaitForSeconds(8f);
        }
        
    }
}