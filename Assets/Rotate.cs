using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 0.5f;

    private void Update ()
    {
        transform.Rotate(Vector3.forward, - Time.fixedDeltaTime * speed * 360);
    }
}
