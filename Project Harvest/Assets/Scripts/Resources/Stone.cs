﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Resource
{
    protected override void Start()
    {
        workerState = Worker.State.StoneMining;

        // pick one of two rocks
        int i = Random.Range(0, 1);
        prop = transform.GetChild(i).gameObject;

        // destroy the other
        Destroy(transform.GetChild(i == 0 ? 1 : 0).gameObject);

        base.Start();
    }
}
