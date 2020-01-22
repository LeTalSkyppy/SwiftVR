using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G2 : Machine
{
    protected override void Start()
    {
        base.Start();
        busy = false;
        timeToProduct = 20f / 3f;
        productTime = timeToProduct;

        productsMachine.Add("G2B", "none");
        productsMachine.Add("G2C", "F4C");
        productsMachine.Add("G2D", "F2D");

        AddProducts();
    }
    protected override void Update()
    {
        base.Update();
    }
}
