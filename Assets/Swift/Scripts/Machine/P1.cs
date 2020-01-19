using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1 : Machine
{
    void Start()
    {
        busy = false;
        timeToProduct = 60f;
        productTime = timeToProduct;

        productsMachine.Add("P1A", "T1A");

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
