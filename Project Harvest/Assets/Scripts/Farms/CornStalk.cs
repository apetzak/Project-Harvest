using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CornStalk : Farm
{
    protected override void Start()
    {
        growthEnd = 900;
        spawnEnd = 90;
        index = 5;
        base.Start();
    }

    protected override void GrowProp()
    {
        prop.transform.localScale += new Vector3(.01f, .01f, .01f);
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
                if (spawnTime > 40)
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

    public override void StartGrowing()
    {
        base.StartGrowing();
        prop.transform.localScale = new Vector3(0, 0, 0);
    }

    public override void StartPicking()
    {
        System.Random rand = new System.Random();
        Pick(rand.Next(2, 4));

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
