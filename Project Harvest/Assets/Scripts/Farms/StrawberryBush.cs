using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawberryBush : Farm
{
    public override void Start()
    {
        growthEnd = 20;
        spawnTime = 90;
        index = 6;
        base.Start();
    }

    public override void GrowProp()
    {
        prop.transform.localScale += new Vector3(.1f, .1f, .1f);
    }

    public override void Update()
    {
        if (state == FarmState.Spawning)
        {
            spawnTime--;
            if (spawnTime <= 0)
            {
                spawnTime = spawnStart;
                state = FarmState.Dead;
                // todo: tip over
            }
            else if (spawnTime > 25)
            {
                foreach (Troop t in troops)
                    t.transform.localScale += new Vector3(2, 2, 2);
            }
        }
        else
        {
            base.Update();
        }
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
            prop.transform.localScale = new Vector3(0, 0, 0);
            state = FarmState.Growing;
        }
        else if (state == FarmState.Pickable)
        {
            troops = Pick(2);
            state = FarmState.Spawning;

            foreach (Troop t in troops)
                t.transform.localScale = new Vector3(0, 0, 0);

            propMesh.enabled = true;
        }
        else if (state == FarmState.Dead)
        {
            propMesh.enabled = false;
            state = FarmState.Empty;
        }

        base.LeftClick();
    }
}
