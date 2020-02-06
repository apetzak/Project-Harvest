using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsparagusPatch : Farm
{
    public override void Start()
    {
        growthEnd = 15;
        prop.transform.Translate(new Vector3(0, -15, 0), Space.World);
        base.Start();
    }

    public override void GrowProp()
    {
        prop.transform.Translate(new Vector3(0, 1, 0), Space.World);
    }

    public override void Update()
    {
        base.Update();
    }

    protected override void LeftClick()
    {
        if (state == FarmState.Spawning)
            return;

        if (state == FarmState.Empty)
        {
            dirtMesh.enabled = true;
            state = FarmState.Planting;
        }
        else if (state == FarmState.Planting)
        {
            propMesh.enabled = true;
            state = FarmState.Growing;
        }
        else if (state == FarmState.Pickable)
        {
            Pick(1);
            prop.transform.Translate(new Vector3(0, -15, 0), Space.World);
        }
    }
}
