using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F1 : Machine
{

    void Start()
    {
        busy = false;
        timeToProduct = 30f;
        productTime = timeToProduct;
        productsMachine.Add("F1A","none");
        productsMachine.Add("F1D","none");

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
