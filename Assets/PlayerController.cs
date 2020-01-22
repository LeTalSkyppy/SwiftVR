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
    [ReadOnly]
    public float runMultiplier = 1f;
    public bool requestCursorLock = true;

    protected Rigidbody rb;
    protected Animator anim;

    protected List<float> cos = new List<float>();
    protected List<float> sin = new List<float>();
    
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
        
        runMultiplier = Mathf.Lerp(runMultiplier, Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? 3 : 1, Time.fixedDeltaTime * 5f);

        Vector3 forwardXZ = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);
        Vector3 dirXZ = x * Camera.main.transform.right + y * forwardXZ * runMultiplier;
        Vector3 lookDirXZ = forwardXZ;

        // set the rotation
        if (dirXZ.magnitude > Mathf.Epsilon)
            transform.LookAt(transform.position + lookDirXZ);

        anim.SetFloat("vertical", y * runMultiplier);
        anim.SetFloat("horizontal", x);

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
