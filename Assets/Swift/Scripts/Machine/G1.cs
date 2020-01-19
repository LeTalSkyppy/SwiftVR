using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G1 : Machine
{
    void Start()
    {
        busy = false;
        timeToProduct = 20f;
        productTime = timeToProduct;

        productsMachine.Add("G1A", "F3A");
        productsMachine.Add("G1C", "T2C");
        productsMachine.Add("G1E", "F3E");

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
