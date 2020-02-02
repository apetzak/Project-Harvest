using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarrotPatch : RootPatch
{
    public override void Start()
    {
        spawnTime = 60;
        growthEnd = 15;
        base.Start();
    }
}
