﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarrotPatch : RootPatch
{
    protected override void Start()
    {
        spawnEnd = 60;
        growthEnd = 15;
        index = 1;
        base.Start();
    }
}
