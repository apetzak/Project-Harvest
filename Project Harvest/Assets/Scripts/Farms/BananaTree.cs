using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaTree : FruitTree
{
    protected override void Start()
    {
        // dont use propMesh
        spawnEnd = 50;
        harvestCount = 3;
        growthEnd = 120;
        plantGrowthEnd = 100;
        plantGrowthIncrement = 1f;
        dirtMesh = dirtMound.GetComponent<MeshRenderer>();
        dirtMesh.enabled = false;
        ShowFruit(false);
        prop.transform.localScale = new Vector3(0, 0, 0);
        stopHeight = 10.5f;
        index = 2;
        health = 300;
    }
}