using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class SimpleDistanceToggle : MonoBehaviour
{

    public GameObject leftSpotlight;
    public GameObject rightSpotlight;
    private Light leftLight;
    private Light rightLight;

    public ActionBasedController leftController;
    public ActionBasedController rightController;

    private float leftDistance = 100f;
    private float rightDistance = 100f;

    private bool leftDistanceInRangeCheck = false;
    private bool rightDistanceInRangeCheck = false;

    // Distance Calculations
    private bool leftCloseEnough = false;
    private bool rightCloseEnough = false;


    // Start is called before the first frame update
    void Start()
    {
        leftLight = leftSpotlight.GetComponent<Light>();
        rightLight = rightSpotlight.GetComponent<Light>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        CalcDistances();
        CheckCloseEnough();
        ToggleSpotlights();
    }

    // --------------------------------------------------------------------------------------------------------

    void CalcDistances()
    {
        leftDistance = transform.position.y - leftController.transform.position.y;
        rightDistance = transform.position.y - rightController.transform.position.y;
    }

    void CheckCloseEnough()
    {

        leftDistanceInRangeCheck = (-0.05f < leftDistance && leftDistance <= 0.05f);
        rightDistanceInRangeCheck = (-0.05f < rightDistance && rightDistance <= 0.05f);

        // Distance check - Left
        if (leftDistanceInRangeCheck && leftCloseEnough)
        { }
        else if (leftDistanceInRangeCheck)
            leftCloseEnough = true;
        else leftCloseEnough = false;

        // Distance check - Right
        if (rightDistanceInRangeCheck && rightCloseEnough)
        { }
        else if (rightDistanceInRangeCheck)
            rightCloseEnough = true;
        else
            rightCloseEnough = false;
    }

    void ToggleSpotlights()
    {
        // Toggle Light - Left
        if (leftCloseEnough && leftLight.enabled)
        { }
        else if (leftCloseEnough)
        {
            leftLight.enabled = true;
            leftSpotlight.transform.GetChild(0).gameObject.SetActive(true);
            Debug.Log("Left turned on");
        }
        else if (!leftCloseEnough && !leftLight.enabled)
        { }
        else
        {
            leftLight.enabled = false;
            leftSpotlight.transform.GetChild(0).gameObject.SetActive(false);
            Debug.Log("Left turned off");
        }

        // Toggle Light - Right
        if (rightCloseEnough && rightLight.enabled)
        { }
        else if (rightCloseEnough)
        {
            rightLight.enabled = true;
            rightSpotlight.transform.GetChild(0).gameObject.SetActive(true);
            Debug.Log("Right turned on");
        }
        else if (!rightCloseEnough && !rightLight.enabled)
        { }
        else
        {
            rightLight.enabled = false;
            rightSpotlight.transform.GetChild(0).gameObject.SetActive(false);
            Debug.Log("Right turned off");
        }
    }
}