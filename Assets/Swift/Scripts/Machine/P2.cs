using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2 : Machine
{
    protected override void Start()
    {
        base.Start();
        busy = false;
        timeToProduct = 60f / 3f;
        productTime = timeToProduct;

        productsMachine.Add("P2B", "F4B");

        AddProducts();
    }
    protected override void Update()
    {
        base.Update();
    }
}
