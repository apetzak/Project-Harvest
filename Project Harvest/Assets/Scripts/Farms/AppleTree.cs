using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : FruitTree
{
    protected override void Start()
    {
        spawnEnd = 50;
        harvestCount = 6;
        growthEnd = 60;
        plantGrowthIncrement = .025f;
        index = 0;
        base.Start();
    }
}
