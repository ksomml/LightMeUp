using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used to easily turn Lights as well as their Lightplanes on and off

public class staticLightToggle
{
    public static void TurnOnLight(Light spotlight1 = null, Light spotlight2 = null)
    {
        if (spotlight1)
        {
            spotlight1.enabled = true;
            spotlight1.transform.GetChild(0).gameObject.SetActive(true);
        }
        if (spotlight2)
        {
            spotlight2.enabled = true;
            spotlight2.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public static void TurnOffLight(Light spotlight1 = null, Light spotlight2 = null)
    {
        if (spotlight1)
        {
            spotlight1.enabled = false;
            spotlight1.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (spotlight2)
        {
            spotlight2.enabled = false;
            spotlight2.transform.GetChild(0).gameObject.SetActive(false);
        }

    }

    public static void ToggleLight(Light spotlight1 = null, Light spotlight2 = null)
    {
        if (spotlight1)
        {
            spotlight1.enabled = !spotlight1.enabled;
            spotlight1.transform.GetChild(0).gameObject.SetActive(!spotlight1.transform.GetChild(0).gameObject.activeSelf);
        }

        if (spotlight2)
        {
            spotlight2.enabled = !spotlight2.enabled;
            spotlight2.transform.GetChild(0).gameObject.SetActive(!spotlight2.transform.GetChild(0).gameObject.activeSelf);
        }
    }


}