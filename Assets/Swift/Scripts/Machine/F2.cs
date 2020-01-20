using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F2 : Machine
{
    // Start is called before the first frame update
    void Start()
    {
        timeToProduct = 20f / 3f;
        productTime = timeToProduct;
        busy = false;

        productsMachine.Add("F2A", "F1A");
        productsMachine.Add("F2B", "T3B");
        productsMachine.Add("F2D", "F3D");

        AddProducts();

    }

    void Update()
    {
        if(isPowered)
        {
            AddQueueProduct();
            BusyOrNotBusy();
        }
    }

}
