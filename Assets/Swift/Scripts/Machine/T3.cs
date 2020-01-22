using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T3 : Machine
{
    protected override void Start()
    {
        base.Start();
        busy = false;
        timeToProduct = 60f/3f;
        productTime = timeToProduct;

        productsMachine.Add("T3B", "G2B");

        AddProducts();
    }
    protected override void Update()
    {
        base.Update();
    }
}
