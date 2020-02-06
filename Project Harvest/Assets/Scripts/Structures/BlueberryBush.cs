using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueberryBush : Hub
{
    void Start()
    {
        growthTimer = growthEnd = 120;
        maxUnits = 8;
        ShowProps(false);
    }

    protected override void GrowUnits()
    {
        props[unitsGrown].GetComponent<MeshRenderer>().enabled = true;
        unitsGrown++;
    }
}
