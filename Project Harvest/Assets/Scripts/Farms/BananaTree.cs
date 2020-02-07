using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaTree : FruitTree
{
    public override void Start()
    {
        // dont use propMesh
        spawnTime = 52;
        harvestCount = 3;
        growthEnd = 120;
        plantGrowthTime = plantGrowthStart = 100;
        plantGrowthIncrement = 1f;
        dirtMesh = dirtMound.GetComponent<MeshRenderer>();
        dirtMesh.enabled = false;
        spawnStart = spawnTime;
        ShowFruit(false);
        prop.transform.localScale = new Vector3(0, 0, 0);
        stopHeight = 15;
        index = 2;
    }
}