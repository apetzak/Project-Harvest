using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootPatch : Farm
{
    private Troop troop;

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
                troop.transform.Translate(new Vector3(0, .5f, 0), Space.World);
            }
            else if (spawnTime > 30)
            {
                // hold still
            }
            else if (spawnTime > 0)
            {
                troop.transform.Translate(new Vector3(0, -.127f, 0), Space.World);
            }
            else
            {
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
            dirtMesh.enabled = true;
            state = State.Planting;
        }
        else if (state == State.Planting)
        {
            propMesh.enabled = true;
            prop.transform.localScale = new Vector3(1, 1, 1);
            state = State.Growing;
        }
        else if (state == State.Pickable)
        {
            troop = Pick(1)[0];
            troop.transform.position = prop.transform.position;
            troop.transform.Translate(new Vector3(0, -5, 0), Space.World);
            state = State.Spawning;
        }

        base.LeftClick();
    }
}
