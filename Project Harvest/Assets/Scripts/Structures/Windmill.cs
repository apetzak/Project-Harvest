using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : Structure
{
    public GameObject vane;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        vane.transform.Rotate(0, 0, 1);
        base.Update();
    }
}
