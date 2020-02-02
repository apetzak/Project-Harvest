using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CornStalk : Farm
{
    public override void Start()
    {
        growthEnd = 90;
        spawnTime = 90;
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
            }
            else
            {
                if (spawnTime > 55)
                {
                    foreach (Troop t in troops)
                        t.transform.localScale += new Vector3(1, 1, 1);
                }
                prop.transform.Rotate(1, 0, 0, Space.Self);
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
            prop.transform.localScale = new Vector3(0,  0, 0);
            state = FarmState.Growing;
        }
        else if (state == FarmState.Pickable)
        {
            System.Random rand = new System.Random();
            troops = Pick(rand.Next(2, 4));

            foreach (Troop t in troops)
                t.transform.localScale = new Vector3(0, 0, 0);

            propMesh.enabled = true;

            state = FarmState.Spawning;
        }
        else if (state == FarmState.Dead)
        {
            propMesh.enabled = false;
            prop.transform.Rotate(-90, 0, 0, Space.Self);
            state = FarmState.Empty;
        }
    }
}
