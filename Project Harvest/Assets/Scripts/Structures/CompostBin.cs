using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompostBin : Structure
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (!isPlaced)
            return;

        base.Update();
    }
}
