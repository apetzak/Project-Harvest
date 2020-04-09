﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnionPatch : RootPatch
{
    protected override void Start()
    {
        spawnEnd = 75;
        growthEnd = 2500;
        index = 9;
        base.Start();
    }
}
