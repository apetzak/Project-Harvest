using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : Structure
{
    public int length;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (!isBuilt)
            return;

        base.Update();
    }
}
