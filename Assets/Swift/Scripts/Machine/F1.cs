using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F1 : Machine
{

    protected override void Start()
    {
        base.Start();
        busy = false;
        timeToProduct = 30f / 3f;
        productTime = timeToProduct;
        productsMachine.Add("F1A","none");
        productsMachine.Add("F1D","none");
        AddProducts();
    }

    protected override void Update()
    {
        base.Update();
    }
}
