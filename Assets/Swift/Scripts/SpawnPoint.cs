using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public static Transform point;

    private void Awake ()
    {
        point = transform;
    }
}
