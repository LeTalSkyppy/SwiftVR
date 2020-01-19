using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P3 : Machine
{
    void Start()
    {
        busy = false;
        timeToProduct = 60f;
        productTime = timeToProduct;

        productsMachine.Add("P3E", "F4E");

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
