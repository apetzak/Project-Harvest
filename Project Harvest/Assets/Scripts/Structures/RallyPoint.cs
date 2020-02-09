using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyPoint : Structure
{
    public Material red;
    public Material green;

    protected override void Start()
    {
        var fabric = transform.GetChild(0).transform.GetChild(0);
        var mr = fabric.GetComponent<SkinnedMeshRenderer>();
        mr.material = fruit ? red : green;
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
