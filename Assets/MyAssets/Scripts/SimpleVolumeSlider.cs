using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public class SimpleVolumeSlider : MonoBehaviour
{
    public InputActionReference toggleReference = null;
    public ActionBasedController leftController;
    public ActionBasedController rightController;

    public static float volumeValue = 0.1f;
    private float vibrationPower = 0.0f;

    private Vector3 sliderAnchor;
    private float maxPosZ;
    private float minPosZ;

    private bool leftInside = false;
    private bool rightInside = false;
    private bool isMovable = false;

    void Start()
    {
        sliderAnchor = gameObject.transform.position;

        maxPosZ = sliderAnchor.z + 0.1f;
        minPosZ = sliderAnchor.z - 0.1f;

        // Event registration
        toggleReference.action.started += StartMovingSlider;
        toggleReference.action.canceled += StopMovingSlider;
    }

    void OnDestroy()
    {
        toggleReference.action.started -= StartMovingSlider;
        toggleReference.action.canceled -= StopMovingSlider;
    }

    void FixedUpdate()
    {
        // clamp slider position
        float clampedZ = Mathf.Clamp(gameObject.transform.position.z, minPosZ, maxPosZ);
        gameObject.transform.position = new Vector3(sliderAnchor.x, sliderAnchor.y, clampedZ);

        // update slider value
        volumeValue = math.remap(maxPosZ, minPosZ, 0f, 1f, gameObject.transform.position.z);

        if (isMovable)
        {
            if (leftInside && !rightInside)
                gameObject.transform.position = new Vector3(sliderAnchor.x, sliderAnchor.y, leftController.transform.position.z);

            if (!leftInside && rightInside)
                gameObject.transform.position = new Vector3(sliderAnchor.x, sliderAnchor.y, rightController.transform.position.z);
        }
    }


    void OnTriggerEnter(Collider controller)
    {
        if (controller.name != "LeftHand Controller" && controller.name != "RightHand Controller")
            return;

        if (controller.name == "LeftHand Controller")
            leftInside = true;

        if (controller.name == "RightHand Controller")
            rightInside = true;

    }

    void OnTriggerStay(Collider controller)
    {
        if (controller.name != "LeftHand Controller" && controller.name != "RightHand Controller")
            return;

        // updating vibration force depending on position
        vibrationPower = math.remap(maxPosZ, minPosZ, 0.1f, 0.6f, gameObject.transform.position.z);

        if (controller.name == "LeftHand Controller" && isMovable)
            leftController.SendHapticImpulse(vibrationPower, 0.1f);


        if (controller.name == "RightHand Controller" && isMovable)
            rightController.SendHapticImpulse(vibrationPower, 0.1f);

    }

    void OnTriggerExit(Collider controller)
    {
        if (controller.name != "LeftHand Controller" && controller.name != "RightHand Controller")
            return;

        if (controller.name == "LeftHand Controller")
            leftInside = false;

        if (controller.name == "RightHand Controller")
            rightInside = false;
    }

    void StartMovingSlider(InputAction.CallbackContext context)
    {
        isMovable = true;
    }

    void StopMovingSlider(InputAction.CallbackContext context)
    {
        isMovable = false;
    }
}
