﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueberryBush : Hub
{
    protected override void Start()
    {
        growthTimer = growthEnd = 100;
        maxUnits = 8;
        ShowProps(false);
        workerPrefab = Assets.Instance.bb;
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void GrowUnits()
    {
        props[unitsGrown].GetComponent<MeshRenderer>().enabled = true;
        unitsGrown++;
    }
}
