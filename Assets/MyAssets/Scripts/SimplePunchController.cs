using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SimplePunchController : MonoBehaviour
{
    public bool doubleHandSupport;
    public ActionBasedController leftController;
    public ActionBasedController rightController;
    public GameObject leftSpotlightModel;
    public GameObject rightSpotlightModel;
    private Transform leftSpotlightTransform;
    private Transform rightSpotlightTransform;

    // using these to make sure that spotlights which are also used by SimpleMovementController are aligned before using with SimplePunchController
    private Vector3 leftSpotlightAnchorPosition;
    private Quaternion leftSpotlightAnchorRotation;
    private Vector3 rightSpotlightAnchorPosition;
    private Quaternion rightSpotlightAnchorRotation;

    private Light leftSpotlightLight;
    private Light rightSpotlightLight;


    void Start()
    {
        // check if spotlights have been inputted
        if (leftSpotlightModel)
        {
            leftSpotlightTransform = leftSpotlightModel.GetComponent<Transform>();
            leftSpotlightAnchorPosition = leftSpotlightTransform.position;
            leftSpotlightAnchorRotation = leftSpotlightTransform.rotation;
            leftSpotlightLight = leftSpotlightModel.transform.Find("DirectionControl/Spotlight").GetComponent<Light>();
        }

        if (rightSpotlightModel)
        {
            rightSpotlightTransform = rightSpotlightModel.GetComponent<Transform>();
            rightSpotlightAnchorPosition = rightSpotlightTransform.position;
            rightSpotlightAnchorRotation = rightSpotlightTransform.rotation;
            rightSpotlightLight = rightSpotlightModel.transform.Find("DirectionControl/Spotlight").GetComponent<Light>();
        }

        staticLightToggle.TurnOffLight(leftSpotlightLight, rightSpotlightLight);
    }

    void OnTriggerEnter(Collider controller)
    {
        if (controller.name != "LeftHand Controller" && controller.name != "RightHand Controller")
            return;

        if (doubleHandSupport && (controller.name == "LeftHand Controller"))
        {
            if (leftSpotlightTransform)
            {
                leftSpotlightLight.color = Color.yellow;
                leftSpotlightTransform.position = leftSpotlightAnchorPosition;
                leftSpotlightTransform.rotation = leftSpotlightAnchorRotation;
            }

            staticLightToggle.TurnOnLight(leftSpotlightLight);

            if (leftController)
                leftController.SendHapticImpulse(0.6f, 0.1f);
        }

        if (doubleHandSupport && (controller.name == "RightHand Controller"))
        {
            if (rightSpotlightTransform)
            {
                rightSpotlightLight.color = Color.yellow;
                rightSpotlightTransform.position = rightSpotlightAnchorPosition;
                rightSpotlightTransform.rotation = rightSpotlightAnchorRotation;
            }

            staticLightToggle.TurnOnLight(rightSpotlightLight);

            if (rightController)
                rightController.SendHapticImpulse(0.6f, 0.1f);
        }

        if (!doubleHandSupport)
        {
            if (leftSpotlightTransform)
            {
                leftSpotlightLight.color = Color.yellow;
                leftSpotlightTransform.position = leftSpotlightAnchorPosition;
                leftSpotlightTransform.rotation = leftSpotlightAnchorRotation;
            }

            if (rightSpotlightTransform)
            {
                rightSpotlightLight.color = Color.yellow;
                rightSpotlightTransform.position = rightSpotlightAnchorPosition;
                rightSpotlightTransform.rotation = rightSpotlightAnchorRotation;
            }

            staticLightToggle.TurnOnLight(leftSpotlightLight, rightSpotlightLight);

            if (leftController && (controller.name == "LeftHand Controller"))
                leftController.SendHapticImpulse(0.6f, 0.1f);
            if (rightController && (controller.name == "RightHand Controller"))
                rightController.SendHapticImpulse(0.6f, 0.1f);
        }

    }

    void OnTriggerExit(Collider controller)
    {
        if (controller.name != "LeftHand Controller" && controller.name != "RightHand Controller")
            return;

        if (doubleHandSupport && (controller.name == "LeftHand Controller"))
            staticLightToggle.TurnOffLight(leftSpotlightLight);
        if (doubleHandSupport && (controller.name == "RightHand Controller"))
            staticLightToggle.TurnOffLight(rightSpotlightLight);
        if (!doubleHandSupport)
            staticLightToggle.TurnOffLight(leftSpotlightLight, rightSpotlightLight);
    }

}
