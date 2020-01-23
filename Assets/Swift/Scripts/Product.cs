using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    public GameObject target;
    Vector3 diff = Vector3.zero;
    Vector3 targetPosition = Vector3.zero;

    Vector3 initialPosition = Vector3.zero;

    float diffSpeed;

    public const float speed = 30f;
    void Start()
    {
        initialPosition = transform.position;
    }
    void Update()
    {
        if(target != null)
        {
            if(targetPosition == Vector3.zero)
            {
                targetPosition = target.transform.position;
                diff = targetPosition - initialPosition;
                diffSpeed = diff.magnitude;
            }

            if(diff == Vector3.zero)
            {
                diff = targetPosition - initialPosition;
                diffSpeed = diff.magnitude;
            }

            if(targetPosition != target.transform.position)
            {
                targetPosition = target.transform.position;
                diff = targetPosition - initialPosition;
            }

            Vector3 diffInstantT = targetPosition - transform.position;            
            Vector3 diffCalcul = diff *(diffSpeed/diff.magnitude);
            transform.position += diffCalcul/100f * speed * Time.deltaTime;

            if(diffInstantT.magnitude < 0.2f)
            {
                Destroy(gameObject);
            }
        }
    }
}
