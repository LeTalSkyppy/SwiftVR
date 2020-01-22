using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G1 : Machine
{
    protected override void Start()
    {
        base.Start();
        busy = false;
        timeToProduct = 20f / 3f;
        productTime = timeToProduct;

        productsMachine.Add("G1A", "F3A");
        productsMachine.Add("G1C", "T2C");
        productsMachine.Add("G1E", "F3E");

        AddProducts();
    }
    protected override void Update()
    {
        base.Update();
    }
}
