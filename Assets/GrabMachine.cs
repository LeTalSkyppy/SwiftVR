using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GrabMachine : MonoBehaviour
{
    SteamVR_Input_Sources inputSource;
    SteamVR_Behaviour_Pose pose;

    private bool isGrabbing = false;

    private bool rotateH = false;

    private bool rotateAH = false;
    private TeleportArc teleportArc = null;

    private GameObject grabObject;

    private float speedoRotato = 100f;

    private bool displayArc = false;
    ControllerPointer controllerPointer;

    public LayerMask layerMask;

    public int layerMaskMachine;
    void Start()
    {
        layerMaskMachine = LayerMask.GetMask("MovableArea");
    }

    void Update()
    {
        if(SteamVR_Actions._default.GrabMachine.GetStateDown(inputSource))
        {
            isGrabbing = true;
            GrabPressed();
        }

        if(SteamVR_Actions._default.GrabMachine.GetStateUp(inputSource))
        {
            isGrabbing = false;
            GrabReleased();
        }

        if(SteamVR_Actions._default.RotateHoraire.GetStateDown(inputSource))
        {
            rotateH = true;
        }

        if(SteamVR_Actions._default.RotateHoraire.GetStateUp(inputSource))
        {
            rotateH = false;
        }

        if(SteamVR_Actions._default.RotateAntiHoraire.GetStateDown(inputSource))
        {
            rotateAH = true;
        }

        if(SteamVR_Actions._default.RotateAntiHoraire.GetStateUp(inputSource))
        {
            rotateAH = false;
        }

        if(rotateH)
        {
            if(grabObject != null)
            {
                Vector3 rotation = new Vector3(0, speedoRotato * Time.deltaTime,0);
                grabObject.transform.Rotate(rotation);
            }
        }

        if(rotateAH)
        {
            if(grabObject != null)
            {
                Vector3 rotation = new Vector3(0, -speedoRotato * Time.deltaTime,0);
                grabObject.transform.Rotate(rotation);
            }
        }

        if(grabObject != null)
        {
            Ray raycast = new Ray(transform.position, transform.forward);
            RaycastHit hitObject;
            if(Physics.Raycast(raycast, out hitObject, 100f, layerMaskMachine))
            {
                Vector3 position = new Vector3(hitObject.point.x, grabObject.transform.position.y, hitObject.point.z);
                grabObject.transform.position = position;
            }
            if(displayArc)
            {
                UpdateArc();
            }
        }
    }

    void Awake()
    {
        pose = GetComponent<SteamVR_Behaviour_Pose>();
        inputSource =  pose.inputSource;
        controllerPointer = gameObject.AddComponent<ControllerPointer>();
        teleportArc = GetComponent<TeleportArc>();
        teleportArc.traceLayerMask = layerMask;
    }

    private void GrabPressed()
    {
        if(controllerPointer.CanGrab)
        {
            grabObject = controllerPointer.grabObject;
            controllerPointer.DesactivatePointer();
            Destroy(controllerPointer);

            displayArc = true;

           
        }
    }

    private void GrabReleased()
    {
        if(controllerPointer.outline != null)
        {
            Destroy(controllerPointer.outline);
        }
        if(controllerPointer == null)
        {
            controllerPointer = gameObject.AddComponent<ControllerPointer>();
        }

        if(grabObject != null)
        {
            displayArc = false;
            grabObject = null;
            teleportArc.Hide();
        }

    }

    private void UpdateArc()
    {
        //Vector3 arcPos = startPos + (projectileVelocity * time) + (0.5 * time * time) * gravity = grabOject.gameObject.transform.position;
        Vector3 gravity = Physics.gravity;
        Vector3 projectileVelocity = (grabObject.transform.position - transform.position - 0.5f * gravity);
        //Vector3 diff = transform.position - grabObject.gameObject.transform.position;
        //Vector3 velocity = transform.forward * diff.magnitude;
        teleportArc.SetArcData(transform.position, projectileVelocity, true, false);
        RaycastHit hit;
        teleportArc.DrawArc(out hit);
        teleportArc.SetColor(Color.cyan);

        teleportArc.Show();
    }
}
