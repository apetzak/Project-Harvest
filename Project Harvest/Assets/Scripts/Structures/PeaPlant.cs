﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaPlant : Hub
{
    public GameObject pod1;
    public GameObject pod2;
    public GameObject pod3;

    void Start()
    {
        growthTimer = growthEnd = 300;
        maxUnits = 9;
        HidePods();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void GrowUnits()
    {
        if (unitsGrown == 0)
        {
            pod1.GetComponent<MeshRenderer>().enabled = true;
            unitsGrown = 3;
        }
        else if (unitsGrown == 3)
        {
            pod2.GetComponent<MeshRenderer>().enabled = true;
            unitsGrown = 6;
        }
        else if (unitsGrown == 6)
        {
            pod3.GetComponent<MeshRenderer>().enabled = true;
            unitsGrown = 9;
        }
    }

    protected override void Pick()
    {
        HidePods();
        base.Pick();
    }

    private void HidePods()
    {
        pod1.GetComponent<MeshRenderer>().enabled = 
        pod2.GetComponent<MeshRenderer>().enabled = 
        pod3.GetComponent<MeshRenderer>().enabled = false;
    }
}
