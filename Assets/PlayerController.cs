using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public static PlayerController myPlayer;

    public GameObject orbitCameraPrefab;
    public OrbitCamera orbitCam;
    public GameObject canvasUI;

    [Tooltip("running will double this speed")]
    public float speed = 1f;
    public float runMultiplier = 1f;
    public bool requestCursorLock = true;

    protected Rigidbody rb;
    protected Animator anim;
    
    // is cursor locked
    protected bool isCursorLocked = false;

    private void Awake ()
    {
        if (photonView.IsMine)
        {
            myPlayer = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        if (photonView.IsMine)
        {
            orbitCam = Instantiate(orbitCameraPrefab, transform.position - Vector3.forward, transform.rotation).GetComponent<OrbitCamera>();
            orbitCam.target = transform;
            Instantiate(canvasUI, Vector3.zero, Quaternion.identity);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!photonView.IsMine)
            return;

        UpdateCursor();

        if (!Camera.main)
            return;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 dir = x * Camera.main.transform.right + y * Camera.main.transform.forward;
        Vector3 lookDir = Camera.main.transform.forward;

        Vector3 dirXZ = Vector3.ProjectOnPlane(dir, Vector3.up);
        Vector3 lookDirXZ = Vector3.ProjectOnPlane(lookDir, Vector3.up).normalized;

        float h = 0f;
        float v = 0f;

        if (dirXZ.magnitude > Mathf.Epsilon)
        {
            float speedMagnitude = dirXZ.magnitude;
            Vector3 moveDir = dirXZ.normalized;

            float moveAngle = Mathf.Atan2(moveDir.x, moveDir.z);
            float lookAngle = Mathf.Atan2(lookDirXZ.x, lookDirXZ.z);

            float angle = lookAngle - moveAngle;

            v = Mathf.Cos(angle);
            h = Mathf.Sin(angle);
        }

        // set the rotation
        transform.LookAt(transform.position + lookDirXZ);

        runMultiplier = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;

        anim.SetFloat("vertical", v * runMultiplier);
        anim.SetFloat("horizontal", h);

        Vector3 velXZ = Vector3.ProjectOnPlane(rb.velocity, Vector3.up);

        rb.AddForce((dirXZ * speed - velXZ * 0.25f) * rb.mass, ForceMode.Impulse);
    }

    private void LateUpdate()
    {
        // control the camera if cursor is locked or if left mouse clicking
        if (photonView.IsMine)
        {
            orbitCam.CameraUpdate(((!requestCursorLock && Input.GetMouseButton(0)) || isCursorLocked));
        }
    }

    protected void UpdateCursor()
    {
        // unlock the cursor when the escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isCursorLocked = false;
        }
        // lock the cursor when the user click on the game if the option is activated
        else if (Input.GetMouseButtonDown(0) && requestCursorLock)
        {
            isCursorLocked = true;
        }

        // update cursor state and visibility
        if (isCursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
