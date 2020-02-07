using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawberryBush : Farm
{
    protected override void Start()
    {
        growthEnd = 20;
        spawnTime = 90;
        index = 6;
        base.Start();
    }

    protected override void GrowProp()
    {
        prop.transform.localScale += new Vector3(.1f, .1f, .1f);
    }

    protected override void Update()
    {
        if (state == State.Spawning)
        {
            spawnTime--;
            if (spawnTime <= 0)
            {
                spawnTime = spawnStart;
                state = State.Dead;
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
            prop.transform.localScale = new Vector3(0, 0, 0);
            state = State.Growing;
        }
        else if (state == State.Pickable)
        {
            troops = Pick(2);
            state = State.Spawning;

            foreach (Troop t in troops)
                t.transform.localScale = new Vector3(0, 0, 0);

            propMesh.enabled = true;
        }
        else if (state == State.Dead)
        {
            propMesh.enabled = false;
            state = State.Empty;
        }

        base.LeftClick();
    }
}
