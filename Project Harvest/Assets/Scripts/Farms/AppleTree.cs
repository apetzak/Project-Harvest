using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : FruitTree
{
    protected override void Start()
    {
        spawnEnd = 50;
        harvestCount = 6;
        growthEnd = 600;
        plantGrowthIncrement = .0025f;
        //Debug.Log("apple tree");
        base.Start();
    }
}
