using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnionPatch : RootPatch
{
    public override void Start()
    {
        spawnTime = 55;
        growthEnd = 25;
        index = 9;
        base.Start();
    }
}
