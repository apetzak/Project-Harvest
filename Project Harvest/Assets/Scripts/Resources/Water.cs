using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Resource
{
    protected override void Start()
    {
        workerState = Worker.State.GatheringWater;
        base.Start();
    }

    private void Update()
    {
        
    }
}
