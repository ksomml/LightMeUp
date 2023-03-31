using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;


public class SimpleMovementMapper : MonoBehaviour
{
    public InputActionReference toggleReference = null;
    public bool pitchMovement;

    public ActionBasedController gameController;
    private Collider gameControllerCollider;
    private TrailRenderer controllerTrail;

    public GameObject spotlightModel;
    private Transform spotlightModelTransform;
    private Transform directionControlTransform;
    private Light spotlightLight;
    private Vector3 spotlightAnchorPosition;
    private Quaternion spotlightAnchorRotation;

    public float rotationOffsetY = 0f;
    public float rotationOffsetZ = 0f;

    private Transform cubeTransform;
    private Vector3 controllerToCubeCenterDistance;

    public float translationFactor = 80f;
    public Vector3 rotationPivot;

    private bool isInside = false;

    static int leftColliderCounter = 0;
    static int rightColliderCounter = 0;


    void Start()
    {
        // Get components
        gameControllerCollider = gameController.GetComponent<Collider>();
        spotlightModelTransform = spotlightModel.GetComponent<Transform>();
        cubeTransform = GetComponent<Transform>();
        directionControlTransform = spotlightModel.transform.Find("DirectionControl").transform;
        spotlightLight = spotlightModel.transform.Find("DirectionControl/Spotlight").GetComponent<Light>();
        controllerTrail = gameController.GetComponent<TrailRenderer>();

        // Event registration
        toggleReference.action.started += controllerPressedGripButton;
        toggleReference.action.canceled += controllerReleasedGripButton;

        // Initial setup
        spotlightAnchorPosition = spotlightModelTransform.position;
        spotlightAnchorRotation = spotlightModelTransform.rotation;
        staticLightToggle.TurnOffLight(spotlightLight);
    }

    void OnDestroy()
    {
        toggleReference.action.started -= controllerPressedGripButton;
        toggleReference.action.canceled -= controllerReleasedGripButton;
    }

    void OnTriggerEnter(Collider controller)
    {
        if (!(controller == gameControllerCollider))
            return;

        isInside = true;

        if (controller.name == "LeftHand Controller")
            leftColliderCounter++;

        if (controller.name == "RightHand Controller")
            rightColliderCounter++;

        Debug.Log("LeftControllerColliderCount: " + leftColliderCounter + "\tRightControllerColliderCount: " + rightColliderCounter);

        controllerTrail.enabled = true;

        if (pitchMovement)
            spotlightLight.color = Color.green;
        else
            spotlightLight.color = Color.blue;

        staticLightToggle.TurnOnLight(spotlightLight);
    }

    void OnTriggerStay(Collider controller)
    {
        // is-controller check:
        if (!(controller == gameControllerCollider))
            return;

        gameController.SendHapticImpulse(0.1f, 0.2f);

        CalcDistance();
        MoveSpotlight();
    }

    void OnTriggerExit(Collider controller)
    {
        if (!(controller == gameControllerCollider))
            return;

        isInside = false;

        if (controller.name == "LeftHand Controller")
            leftColliderCounter--;

        if (controller.name == "RightHand Controller")
            rightColliderCounter--;

        Debug.Log("LeftControllerColliderCount: " + leftColliderCounter + "\tRightControllerColliderCount: " + rightColliderCounter);

        if (controller.name == "LeftHand Controller" && leftColliderCounter <= 0)
        {
            controllerTrail.enabled = false;
            Debug.Log("LeftHand Controller disabled!");
        }

        if (controller.name == "RightHand Controller" && rightColliderCounter <= 0)
        {
            controllerTrail.enabled = false;
            Debug.Log("RightHand Controller disabled!");
        }

        staticLightToggle.TurnOffLight(spotlightLight);
    }

    void controllerPressedGripButton(InputAction.CallbackContext context)
    {
        if (isInside)
            staticLightToggle.TurnOffLight(spotlightLight);
    }

    void controllerReleasedGripButton(InputAction.CallbackContext context)
    {
        // there was an issue with the staticLightToggle toggling all lights in combination with the toggleReference.action.canceled event
        // even though it shouldn't have access, so I used this method as a quick fix
        FixFunction();
    }

    void FixFunction()
    {
        if (isInside)
            staticLightToggle.TurnOnLight(spotlightLight);
    }

    void CalcDistance()
    {
        controllerToCubeCenterDistance.x = Vector3.Dot(cubeTransform.forward, gameController.transform.position - cubeTransform.position);
    }

    void MoveSpotlight()
    {
        // Position - Movement
        spotlightModelTransform.position = spotlightAnchorPosition;
        spotlightModelTransform.rotation = spotlightAnchorRotation;

        if (pitchMovement)
            spotlightModel.transform.RotateAround(spotlightAnchorPosition + rotationPivot, Vector3.left, (translationFactor * controllerToCubeCenterDistance.x));
        else
        {
            spotlightModel.transform.RotateAround(spotlightAnchorPosition + rotationPivot, Vector3.up, (translationFactor * controllerToCubeCenterDistance.x));

            // Rotation - Movement
            Vector3 currentRotation = gameController.transform.eulerAngles;
            currentRotation.y += rotationOffsetY;
            currentRotation.z += rotationOffsetZ;
            directionControlTransform.eulerAngles = currentRotation;
        }

        // TO-DO: implement rotation limit

    }


    // EXTRA ------------------------------------------------------------------

    // for rotation center visualization
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spotlightAnchorPosition + rotationPivot, 0.1f);
    }


    //Funny circular Motion -> controller like car wheelpaddle 
    void MoveSpotlight2()
    {
        spotlightModelTransform.rotation = Quaternion.identity;

        spotlightModel.transform.RotateAround(spotlightAnchorPosition + rotationPivot, Vector3.up, (translationFactor * controllerToCubeCenterDistance.x));
    }
}
