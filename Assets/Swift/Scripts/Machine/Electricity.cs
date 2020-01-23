using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Machine>())
        {
            Machine machine = other.gameObject.GetComponent<Machine>();
            machine.isPowered = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<Machine>())
        {
            Machine machine = other.gameObject.GetComponent<Machine>();
            machine.isPowered = false;
        }
    }
}
