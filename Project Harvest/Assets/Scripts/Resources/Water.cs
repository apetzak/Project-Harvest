﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Resource
{
    protected override void Start()
    {
        workerstate = Worker.State.CollectingWater;
        base.Start();
    }

    private void Update()
    {
        
    }
}
