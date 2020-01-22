using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1 : Machine
{
    protected override void Start()
    {
        base.Start();
        busy = false;
        timeToProduct = 60f / 3f;
        productTime = timeToProduct;

        productsMachine.Add("P1A", "T1A");

        AddProducts();
    }
    protected override void Update()
    {
        base.Update();
    }
}
