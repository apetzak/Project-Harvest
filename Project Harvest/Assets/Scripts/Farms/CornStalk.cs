using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CornStalk : Farm
{
    protected override void Start()
    {
        growthEnd = 90;
        spawnTime = 90;
        index = 5;
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
                MoveToRallyPoint();
                spawnTime = spawnStart;
                state = State.Dead;
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
            prop.transform.localScale = new Vector3(0,  0, 0);
        }
        else if (state == State.Pickable)
        {
            System.Random rand = new System.Random();
            Pick(rand.Next(2, 4));

            foreach (Troop t in troops)
                t.transform.localScale = new Vector3(0, 0, 0);

            StartSpawning();
        }
        else if (state == State.Dead)
        {
            prop.transform.Rotate(-90, 0, 0, Space.Self);
            Clear();
        }

        base.LeftClick();
    }
}
