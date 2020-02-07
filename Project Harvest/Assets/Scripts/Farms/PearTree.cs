using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearTree : FruitTree
{
    public override void Start()
    {
        spawnTime = 65;
        harvestCount = 5;
        growthEnd = 80;
        plantGrowthIncrement = .025f;
        index = 4;
        base.Start();
    }
}
