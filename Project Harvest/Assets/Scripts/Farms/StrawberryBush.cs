using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawberryBush : Farm
{
    protected override void Start()
    {
        growthEnd = 20;
        spawnEnd = 90;
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
            spawnTime++;
            if (spawnTime >= spawnEnd)
            {
                MoveToRallyPoint();
                spawnTime = 0;
                state = State.Dead;
            }
            else
            {
                if (spawnTime < 50)
                {
                    foreach (Troop t in troops)
                        t.transform.localScale += new Vector3(.02f, .02f, .02f);
                }
                prop.transform.Rotate(1, 0, 0, Space.Self);
            }
        }
        else
        {
            base.Update();
        }
    }

    protected override void RightClick()
    {
        if (state == State.Spawning)
            return;

        if (state == State.Empty)
        {
            StartPlanting();
        }
        else if (state == State.Planting)
        {
            prop.transform.localScale = new Vector3(0, 0, 0);
            StartGrowing();
        }
        else if (state == State.Pickable)
        {
            Pick(2);

            foreach (Troop t in troops)
                t.transform.localScale = new Vector3(0, 0, 0);

            StartSpawning();
        }
        else if (state == State.Dead)
        {
            prop.transform.Rotate(-90, 0, 0, Space.Self);
            Clear();
        }

        base.RightClick();
    }
}
