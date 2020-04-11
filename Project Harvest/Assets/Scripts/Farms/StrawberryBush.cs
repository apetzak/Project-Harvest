using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawberryBush : Farm
{
    protected override void Start()
    {
        growthEnd = 2000;
        spawnEnd = 90;
        base.Start();
    }

    protected override void GrowProp()
    {
        prop.transform.localScale += new Vector3(.001f, .001f, .001f);
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
                prop.transform.Rotate(1, 0, 0, Space.Self); // fall down
            }
        }
        else
        {
            base.Update();
        }
    }

    public override void StartGrowing()
    {
        prop.transform.localScale = new Vector3(0, 0, 0);
        base.StartGrowing();
    }

    public override void StartPicking()
    {
        Pick(2);

        foreach (Troop t in troops)
            t.transform.localScale = new Vector3(0, 0, 0);

        StartSpawning();
    }

    protected override void RightClick()
    {
        //if (state == State.Dead)
        //{
        //    prop.transform.Rotate(-90, 0, 0, Space.Self);
        //    Clear();
        //}

        base.RightClick();
    }
}
