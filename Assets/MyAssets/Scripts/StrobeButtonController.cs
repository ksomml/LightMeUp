using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class StrobeButtonController : MonoBehaviour
{
    public GameObject strobeModel;
    private Light strobeModelLight;

    public ActionBasedController leftController;
    public ActionBasedController rightController;

    private bool isTriggered = false;
    private int totalHandsInside = 0;

    void Start()
    {
        if (strobeModel)
            strobeModelLight = strobeModel.transform.Find("DirectionControl/Spotlight").GetComponent<Light>();
    }

    void OnTriggerEnter(Collider controller)
    {
        if (controller.name != "LeftHand Controller" && controller.name != "RightHand Controller")
            return;

        totalHandsInside += 1;
        isTriggered = true;

        StartCoroutine(ToggleLight(controller));
    }
    void OnTriggerExit(Collider controller)
    {

        if (controller.name != "LeftHand Controller" && controller.name != "RightHand Controller")
            return;

        totalHandsInside -= 1;

        if (totalHandsInside == 0)
        {
            isTriggered = false;
            StopCoroutine(ToggleLight(controller));
            staticLightToggle.TurnOffLight(strobeModelLight);
        }
    }

    IEnumerator ToggleLight(Collider controller)
    {
        while (isTriggered)
        {
            staticLightToggle.ToggleLight(strobeModelLight);

            if (controller.name == "LeftHand Controller")
                leftController.SendHapticImpulse(0.3f, 0.1f);

            if (controller.name == "RightHand Controller")
                rightController.SendHapticImpulse(0.3f, 0.1f);

            yield return new WaitForSeconds(StrobeSliderController.sliderValue);
        }
    }
}
