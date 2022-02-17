using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    //-----TRAFFIC LIGHT-----//
    //traffic light1
    public GameObject lights1 = null;
    public static bool isRedLight1 = false;
    public static bool isYellowLight1 = false;
    public static bool isGreenLight1 = false;
    //traffic light2
    public GameObject lights2 = null;
    public static bool isRedLight2 = false;
    public static bool isYellowLight2 = false;
    public static bool isGreenLight2 = false;
    //lights color
    private Material red;
    private Material amber;
    private Material green;
    private Material redLight;
    private Material yellowLight;
    private Material greenLight;

    //-----Human-----//
    public GameObject human = null;
    public Renderer humanColor = null;

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
        //-----TRAFFIC LIGHT-----//
        red = Resources.Load<Material>("Red");
        amber = Resources.Load<Material>("Amber");
        green = Resources.Load<Material>("Green");
        redLight = Resources.Load<Material>("RedLight");
        yellowLight = Resources.Load<Material>("YellowLight");
        greenLight = Resources.Load<Material>("GreenLight");
        lights1 = GameObject.Find("Lights1");
        lights2 = GameObject.Find("Lights2");
        StartCoroutine("TrafficControl");

        //-----Human-----//
        human = GameObject.Find("Human");
        humanColor = human.GetComponent<Renderer>();
        StartCoroutine("HumanMoving");
    }

    // Update is called once per frame
    void Update()
    {
        if (isRedLight1 == true)
        {
            TurnOnRed(lights1);
        }
        if (isYellowLight1 == true)
        {
            TurnOnYellow(lights1);
        }
        if (isGreenLight1 == true)
        {
            TurnOnGreen(lights1);
        }
        if (isRedLight2 == true)
        {
            TurnOnRed(lights2);
        }
        if (isYellowLight2 == true)
        {
            TurnOnYellow(lights2);
        }
        if (isGreenLight2 == true)
        {
            TurnOnGreen(lights2);
        }
        if (human.transform.eulerAngles.y < -5f || human.transform.eulerAngles.y > 5f)
        {
            humanColor.material.color = Color.red;
        }
    }

    private void TurnOnRed(GameObject lights)
    {
        Material[] material = lights.GetComponent<MeshRenderer>().materials;
        material[0] = amber;
        material[1] = redLight;
        material[2] = green;
        lights.GetComponent<MeshRenderer>().materials = material;
    }
    private void TurnOnYellow(GameObject lights)
    {
        Material[] material = lights.GetComponent<MeshRenderer>().materials;
        material[0] = yellowLight;
        material[1] = red;
        material[2] = green;
        lights.GetComponent<MeshRenderer>().materials = material;
    }
    private void TurnOnGreen(GameObject lights)
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
            isRedLight1 = false;
            isYellowLight1 = false;
            isGreenLight1 = true;

            isRedLight2 = true;
            isYellowLight2 = false;
            isGreenLight2 = false;
            yield return new WaitForSeconds(10f);

            isRedLight1 = false;
            isYellowLight1 = true;
            isGreenLight1 = false;

            isRedLight2 = false;
            isYellowLight2 = true;
            isGreenLight2 = false;
            yield return new WaitForSeconds(1.5f);

            isRedLight1 = true;
            isYellowLight1 = false;
            isGreenLight1 = false;

            isRedLight2 = false;
            isYellowLight2 = false;
            isGreenLight2 = true;
            yield return new WaitForSeconds(10f);

            isRedLight1 = false;
            isYellowLight1 = true;
            isGreenLight1 = false;

            isRedLight2 = false;
            isYellowLight2 = true;
            isGreenLight2 = false;
            yield return new WaitForSeconds(1.5f);
        }
        
    }

    IEnumerator HumanMoving()
    {
        bool isMoveRight = true;
        yield return new WaitForSeconds(3.5f);
        while (true)
        {
            if (human.transform.position.x > -8f && isMoveRight == false)
            {
                break;
            }
            if (isMoveRight == true)
            {
                human.transform.position = human.transform.position + new Vector3(-15 * Time.deltaTime, 0, 0);
                if (human.transform.position.x < -270f)
                {
                    isMoveRight = false;
                }
                yield return null;
            }
            if (isMoveRight == false)
            {
                human.transform.position = human.transform.position + new Vector3(20 * Time.deltaTime, 0, 0);
                yield return null;
            }
        }
    }
}