using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsparagusPatch : Farm
{
    protected override void Start()
    {
        growthEnd = 1500;
        prop.transform.Translate(new Vector3(0, -15, 0), Space.World);
        base.Start();
    }

    protected override void GrowProp()
    {
        prop.transform.Translate(new Vector3(0, .01f, 0), Space.World);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void StartPicking()
    {
        Pick(1);
        MoveToRallyPoint();
        prop.transform.Translate(new Vector3(0, -15, 0), Space.World);
        state = State.Dead;
    }

    protected override void RightClick()
    {
        base.RightClick();
    }
}
