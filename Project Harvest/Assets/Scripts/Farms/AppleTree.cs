using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : FruitTree
{
    public override void Start()
    {
        spawnTime = 50;
        harvestCount = 6;
        growthEnd = 60;
        plantGrowthIncrement = .025f;
        base.Start();
    }
}
