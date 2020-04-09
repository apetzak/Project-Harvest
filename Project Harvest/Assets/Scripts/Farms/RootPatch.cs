using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootPatch : Farm
{
    protected override void GrowProp()
    {
        prop.transform.localScale += new Vector3(.01f, .01f, .01f);
    }

    protected override void Update()
    {
        if (state == State.Spawning)
        {
            spawnTime++;

            if (spawnTime < 25)
            {
                troops[0].transform.Translate(new Vector3(0, .5f, 0), Space.World);
            }
            else if (spawnTime < 30)
            {
                // hold still
            }
            else if (spawnTime < spawnEnd)
            {
                troops[0].transform.Translate(new Vector3(0, -.127f, 0), Space.World);
            }
            else
            {
                MoveToRallyPoint();
                spawnTime = 0;
                state = State.Dead;
            }
        }
        else
        {
            base.Update();
        }
    }

    public override void StartGrowing()
    {
        prop.transform.localScale = new Vector3(1, 1, 1);
        base.StartGrowing();
    }

    public override void StartPicking()
    {
        Pick(1);
        troops[0].transform.position = prop.transform.position;
        troops[0].transform.Translate(new Vector3(0, -5, 0), Space.World);
        state = State.Spawning;
    }

    protected override void RightClick()
    {
        base.RightClick();
    }
}
