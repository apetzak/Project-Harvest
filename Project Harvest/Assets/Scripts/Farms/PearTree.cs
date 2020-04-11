using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearTree : FruitTree
{
    protected override void Start()
    {
        spawnEnd = 50;
        harvestCount = 5;
        growthEnd = 600;
        plantGrowthIncrement = .0025f;
        base.Start();
    }
}
