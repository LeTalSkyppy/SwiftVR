using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerInput : MonoBehaviour
{
    public delegate void OnGrabPressed(GameObject controller);
    public static event OnGrabPressed onGrabPressed;
    public delegate void OnGrabReleased(GameObject controller);
    public static event OnGrabReleased onGrabReleased;
    SteamVR_Input_Sources inputSource;
    GameObject SelectedObject;
    bool isGrabbingPinch;

    SteamVR_Behaviour_Pose pose;

    ControllerPointer controllerPointer;

    FixedJoint fixedJoint;
    // Start is called before the first frame update
    void Start()
    {
        fixedJoint = gameObject.AddComponent<FixedJoint>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SteamVR_Actions._default.GrabPinch.GetStateDown(inputSource))
        {
            if(SelectedObject != null && fixedJoint.connectedBody == null)
            {
                GrabSelectedObject();
            }

            if(onGrabPressed != null)
            {
                onGrabPressed(gameObject);
            }
            isGrabbingPinch = true;
            //Debug.Log(inputSource + "Grabbing pinch : " + isGrabbingPinch );
        }
        
        if(SteamVR_Actions._default.GrabPinch.GetStateUp(inputSource))
        {
            isGrabbingPinch = false;
            if(SelectedObject != null)
            {
                UngrabTouchedObject();
            }

            if(onGrabReleased != null)
            {
                onGrabReleased(gameObject);
            }
        }

        if(SteamVR_Actions._default.Teleport.GetStateDown(inputSource))
        {
            TeleportPressed();
        }

        if(SteamVR_Actions._default.Teleport.GetStateUp(inputSource))
        {
            if(controllerPointer != null)
            {
                TeleportReleased();
            }
        }
    }

    void Awake()
    {
        pose = GetComponent<SteamVR_Behaviour_Pose>();
        inputSource =  pose.inputSource;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GrabbableObject>())
        {
            SelectedObject= other.gameObject;
        } 
    }

  private void OnTriggerExit(Collider other)
    {
        if (SelectedObject == other.gameObject)
        {
            SelectedObject = null;
        }
    }

    private void GrabSelectedObject()
    {
        fixedJoint.breakForce = 20000;
        fixedJoint.breakTorque = 20000;
        fixedJoint.connectedBody = SelectedObject.GetComponent<Rigidbody>();
    }

    private void UngrabTouchedObject()
    {
        if(fixedJoint.connectedBody != null)
        {
            fixedJoint.connectedBody = null;
        }

        Vector3 velocity = pose.GetVelocity();
        Vector3 angularVelocity = pose.GetAngularVelocity();

        Rigidbody body = SelectedObject.GetComponent<Rigidbody>();
        body.velocity = velocity;
        body.angularVelocity = angularVelocity;

    }

    private void TeleportPressed()
    {
        if(controllerPointer == null)
        {
            controllerPointer = gameObject.AddComponent<ControllerPointer>();
        }
        controllerPointer.UpdateColor(Color.green);
    }

    private void TeleportReleased()
    {
        if(controllerPointer.CanTeleport)
        {
            GameObject camera = GameObject.Find("Camera");
            GameObject cameraRIG = GameObject.Find("[CameraRig]");

            camera.transform.position = controllerPointer.TargetPosition;
            cameraRIG.transform.position = controllerPointer.TargetPosition;
            
        }

        controllerPointer.DesactivatePointer();

        Destroy(controllerPointer);
    }
}
