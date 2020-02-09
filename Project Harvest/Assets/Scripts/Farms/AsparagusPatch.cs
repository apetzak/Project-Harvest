using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsparagusPatch : Farm
{
    protected override void Start()
    {
        growthEnd = 15;
        prop.transform.Translate(new Vector3(0, -15, 0), Space.World);
        index = 7;
        base.Start();
    }

    protected override void GrowProp()
    {
        prop.transform.Translate(new Vector3(0, 1, 0), Space.World);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void LeftClick()
    {
        if (state == State.Spawning)
            return;

        if (state == State.Empty)
        {
            StartPlanting();
        }
        else if (state == State.Planting)
        {
            StartGrowing();
        }
        else if (state == State.Pickable)
        {
            Pick(1);
            MoveToRallyPoint();
            prop.transform.Translate(new Vector3(0, -15, 0), Space.World);
        }

        base.LeftClick();
    }
}
