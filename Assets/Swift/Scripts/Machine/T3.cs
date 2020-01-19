using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T3 : Machine
{
    void Start()
    {
        busy = false;
        timeToProduct = 60f;
        productTime = timeToProduct;

        productsMachine.Add("T3B", "G2B");

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
