using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Importation : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown("p"))
        {
            Configuration.Export();
        }

        if(Input.GetKeyDown("i"))
        {
            Configuration.Import();
        }        
    }
}
