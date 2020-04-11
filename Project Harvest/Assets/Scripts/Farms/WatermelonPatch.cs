using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatermelonPatch : Farm
{
    public GameObject spawnPoint;

    protected override void Start()
    {
        growthEnd = 1600;
        spawnEnd = 90;
        prop.transform.localScale = new Vector3(0, 0, 0);
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
            troops[0].transform.Rotate(0, 0, -1, Space.World);

            if (spawnTime >= spawnEnd)
            {
                troops[0].transform.Translate(0, -5f, 0);
                troops[0].facingAngle = 270; // facing east
                spawnTime = 0;
                MoveToRallyPoint();
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
        prop.transform.localScale = new Vector3(0, 0, 0);
        base.StartGrowing();
    }

    public override void StartPicking()
    {
        Pick(1);
        troops[0].transform.position = spawnPoint.transform.position;
        troops[0].transform.rotation = spawnPoint.transform.rotation;
        state = State.Spawning;
    }

    protected override void RightClick()
    {
        //if (state == State.Dead)
        //{
        //    Clear();
        //}

        base.RightClick();
    }
}
