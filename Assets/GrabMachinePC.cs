using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Photon.Pun;

public class GrabMachinePC : MonoBehaviourPunCallbacks
{
    private bool isGrabbing = false;
    private bool rotateH = false;
    private bool rotateAH = false;
    private TeleportArc teleportArc = null;
    private GameObject grabObject;
    private float speedoRotato = 100f;
    private bool displayArc = false;
    ControllerPointer controllerPointer;
    public LayerMask layerMask;

    private GameObject TPCamera;

    public int layerMaskMachine;

    private void Start()
    {   
        if (photonView.IsMine)
        {
            layerMaskMachine = LayerMask.GetMask("MovableArea");
            TPCamera = gameObject.GetComponent<PlayerController>().orbitCam.gameObject;
            controllerPointer = TPCamera.AddComponent<ControllerPointer>();
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            GrabPressed();
        }

        if(Input.GetMouseButtonUp(0))
        {
            GrabReleased();
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
            Ray raycast = new Ray(TPCamera.transform.position, TPCamera.transform.forward);
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
        teleportArc = GetComponent<TeleportArc>();
        teleportArc.traceLayerMask = layerMask;
    }

    private void GrabPressed()
    {
        if(controllerPointer.CanGrab)
        {
            PhotonView objView = controllerPointer.grabObject.GetComponent<PhotonView>();

            if (objView.Owner == PhotonNetwork.LocalPlayer)
            {
                Debug.Log("Already Owner");
            }
            else
            {
                Debug.Log("Request Owner");
                objView.RequestOwnership();
            }
            
            grabObject = controllerPointer.grabObject;
        }
    }

    private void GrabReleased()
    {
        if(controllerPointer.outline != null)
        {
            Destroy(controllerPointer.outline);
        }

        if(grabObject != null)
        {
            grabObject = null;
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
