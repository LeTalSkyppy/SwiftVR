﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Product : MonoBehaviour
{
    public GameObject target;

    public List<Text> textList = new List<Text>();
    Vector3 diff = Vector3.zero;
    Vector3 targetPosition = Vector3.zero;

    Vector3 initialPosition = Vector3.zero;

    float diffSpeed;

    public float speed = 50f;

    public float speedoRotato = 500f;
    void Start()
    {
        initialPosition = transform.position;
    }
    void Update()
    {
        if(target != null)
        {
            if(targetPosition == Vector3.zero || diff == Vector3.zero)
            {
                targetPosition = target.transform.position;
                diff = targetPosition - initialPosition;
                diffSpeed = diff.magnitude;
            }

            if(targetPosition != target.transform.position)
            {
                targetPosition = target.transform.position;
                //diff = targetPosition - initialPosition;
            }

            Vector3 diffInstantT = targetPosition - transform.position;            
            Vector3 diffCalcul = diffInstantT *(diffSpeed/diffInstantT.magnitude);
            transform.position += diffCalcul/100f * speed * Time.deltaTime;
            transform.Rotate(new Vector3(0,speedoRotato * Time.deltaTime,0));
            if(diffInstantT.magnitude < 0.2f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetTypeProduct(string type)
    {
        foreach(Text text in textList)
        {
            text.text = type;
        }
    }
}
