using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootPatch : Farm
{
    protected override void GrowProp()
    {
        prop.transform.localScale += new Vector3(1, 1, 1);
    }

    protected override void Update()
    {
        if (state == State.Spawning)
        {
            spawnTime--;

            if (spawnTime > 35)
            {
                troops[0].transform.Translate(new Vector3(0, .5f, 0), Space.World);
            }
            else if (spawnTime > 30)
            {
                // hold still
            }
            else if (spawnTime > 0)
            {
                troops[0].transform.Translate(new Vector3(0, -.127f, 0), Space.World);
            }
            else
            {
                MoveToRallyPoint();
                spawnTime = spawnStart;
                state = State.Empty;
            }
        }
        else
        {
            base.Update();
        }
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
            prop.transform.localScale = new Vector3(1, 1, 1);
            StartGrowing();
        }
        else if (state == State.Pickable)
        {
            Pick(1);
            troops[0].transform.position = prop.transform.position;
            troops[0].transform.Translate(new Vector3(0, -5, 0), Space.World);
            state = State.Spawning;
        }

        base.LeftClick();
    }
}
