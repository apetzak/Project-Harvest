﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Resource
{
    protected override void Start()
    {
        workerstate = Worker.State.CollectingWater;
        cursorIndex = 3;
        base.Start();
    }

    private void Update()
    {
        
    }
}
