using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T1 : Machine
{
    // Start is called before the first frame update
    void Start()
    {
        busy = false;
        timeToProduct = 30f;
        productTime = timeToProduct;
        
        productsMachine.Add("T1A", "F2A");
        productsMachine.Add("T1D", "F1D");

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
