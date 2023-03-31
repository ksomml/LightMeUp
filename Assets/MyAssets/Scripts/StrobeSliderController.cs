using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public class StrobeSliderController : MonoBehaviour
{
    public InputActionReference toggleReference = null;
    public ActionBasedController leftController;
    public ActionBasedController rightController;

    public static float sliderValue = 0.1f;
    private float vibrationPower = 0.0f;

    private Vector3 sliderAnchor;
    private float bottomMaxPosY;
    private float topMaxPosY;

    private bool leftInside = false;
    private bool rightInside = false;
    private bool isMovable = false;

    void Start()
    {
        sliderAnchor = gameObject.transform.position;

        bottomMaxPosY = sliderAnchor.y;
        topMaxPosY = sliderAnchor.y - 0.2f;

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
        float clampedY = Mathf.Clamp(gameObject.transform.position.y, topMaxPosY, bottomMaxPosY);
        gameObject.transform.position = new Vector3(sliderAnchor.x, clampedY, sliderAnchor.z);

        // update slider value
        sliderValue = math.remap(bottomMaxPosY, topMaxPosY, 0.1f, 0.01f, gameObject.transform.position.y);

        if (isMovable)
        {
            Debug.Log("Following Controller!");
            if (leftInside && !rightInside)
                gameObject.transform.position = new Vector3(sliderAnchor.x, leftController.transform.position.y, sliderAnchor.z);

            if (!leftInside && rightInside)
                gameObject.transform.position = new Vector3(sliderAnchor.x, rightController.transform.position.y, sliderAnchor.z);
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
        vibrationPower = math.remap(bottomMaxPosY, topMaxPosY, 0.1f, 0.6f, gameObject.transform.position.y);

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
