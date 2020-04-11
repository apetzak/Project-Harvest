using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Resource
{
    private int fallCounter = 0;

    protected override void Start()
    {
        workerState = Worker.State.WoodCutting;
        base.Start();
    }

    private void Update()
    {
        if (isDying)
        {
            fallCounter++;
            transform.Rotate(.5f, 0, 0);

            if (fallCounter >= 90)
                Remove();
        }
    }

    public void FallOver()
    {
        isDying = true;
    }
}
