using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F3 : Machine
{

    // Start is called before the first frame update
    void Start()
    {
        busy = false;
        timeToProduct = 15f;
        productTime = timeToProduct;

        productsMachine.Add("F3C", "none");
        productsMachine.Add("F3E", "none");
        productsMachine.Add("F3D", "T1D");
        productsMachine.Add("F3A", "P1A");

        AddProducts();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPowered)
        {
            AddQueueProduct();
            BusyOrNotBusy();
        }
    }
}
