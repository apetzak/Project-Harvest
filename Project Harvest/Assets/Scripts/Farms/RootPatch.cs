using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootPatch : Farm
{
    private Troop troop;

    public override void GrowProp()
    {
        prop.transform.localScale += new Vector3(1, 1, 1);
    }

    public override void Update()
    {
        if (state == FarmState.Spawning)
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
                state = FarmState.Empty;
            }
        }
        else
        {
            base.Update();
        }
    }

    public override void LeftClick()
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
            prop.transform.localScale = new Vector3(1, 1, 1);
            state = FarmState.Growing;
        }
        else if (state == FarmState.Pickable)
        {
            troop = Pick(1)[0];
            troop.transform.position = prop.transform.position;
            troop.transform.Translate(new Vector3(0, -5, 0), Space.World);
            state = FarmState.Spawning;
        }
    }
}
