using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G2 : Machine
{
    void Start()
    {
        busy = false;
        timeToProduct = 20f;
        productTime = timeToProduct;

        productsMachine.Add("G2B", "none");
        productsMachine.Add("G2C", "F4C");
        productsMachine.Add("G2D", "F2D");

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
