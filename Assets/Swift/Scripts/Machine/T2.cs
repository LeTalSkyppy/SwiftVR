using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T2 : Machine
{
    void Start()
    {
        busy = false;
        timeToProduct = 60f/3f;
        productTime = timeToProduct;

        productsMachine.Add("T2C", "F3C");

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
