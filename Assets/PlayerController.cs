using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public OrbitCamera orbitCam;
    [Tooltip("running will double this speed")]
    public float speed = 1f;
    public bool requestCursorLock = true;

    protected CharacterController cc;
    protected Animator anim;
    // list of previous directions to smooth the rotation
    protected List<Vector3> directions = new List<Vector3>();
    // time when start running to smooth in
    protected float runStart;
    // time when stop running to smooth out
    protected float runStop;
    // is cursor locked
    protected bool isCursorLocked = false;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateCursor();

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (!Camera.main)
        {
            return;
        }

        Vector3 dir = x * Camera.main.transform.right + y * Camera.main.transform.forward;

        Vector3 dirXZ = Vector3.ProjectOnPlane(dir, Vector3.up);

        if (dirXZ.magnitude < Mathf.Epsilon)
        {
            anim?.SetFloat("speed", 0f);
            return;
        }

        if (dirXZ.magnitude > 1f)
        {
            dirXZ.Normalize();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            runStart = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            runStop = Time.time;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            dirXZ *= Mathf.Lerp(1, 2, (Time.time - runStart)*2);
        }
        else
        {
            dirXZ *= Mathf.Lerp(2, 1, (Time.time - runStop) * 2);
        }

        anim?.SetFloat("speed", dirXZ.magnitude);

        //cc.SimpleMove(dirXZ * speed);

        directions.Add(dirXZ);
        // smooth the rotation
        if (directions.Count > 5)
        {
            directions.RemoveAt(0);
        }
        // by using the average of the 5 previous values
        Vector3 average = directions.Aggregate(new Vector3(0, 0, 0), (s, v) => s + v) / (float)directions.Count;
        // set the rotation
        transform.LookAt(transform.position + average);
    }

    private void LateUpdate()
    {
        // control the camera if cursor is locked or if left mouse clicking
        orbitCam.CameraUpdate(((!requestCursorLock && Input.GetMouseButton(0)) || isCursorLocked));
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
