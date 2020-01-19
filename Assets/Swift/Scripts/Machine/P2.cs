﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2 : Machine
{
    void Start()
    {
        busy = false;
        timeToProduct = 60f;
        productTime = timeToProduct;

        productsMachine.Add("P2B", "F4B");

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
