﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F4 : Machine
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        busy = false;
        timeToProduct = 20f / 3f;
        productTime = timeToProduct;

        productsMachine.Add("F4B", "F2B");
        productsMachine.Add("F4C", "G1C");
        productsMachine.Add("F4E", "G1E");

        AddProducts();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
