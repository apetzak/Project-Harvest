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
        growthEnd = 1200;
        plantGrowthEnd = 1000;
        plantGrowthIncrement = .1f;
        dirtMesh = dirtMound.GetComponent<MeshRenderer>();
        dirtMesh.enabled = false;
        ShowFruit(false);
        prop.transform.localScale = new Vector3(0, 0, 0);
        stopHeight = 10.5f;
        index = 2;
        health = 300;
        size = GetSize();
        grassCount = 300 * size;
        sproutEnd = 480;
        GetGrassMeshes();
    }
}