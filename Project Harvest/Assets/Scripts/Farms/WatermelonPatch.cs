using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatermelonPatch : Farm
{
    public GameObject spawnPoint;

    protected override void Start()
    {
        growthEnd = 160;
        spawnEnd = 90;
        prop.transform.localScale = new Vector3(0, 0, 0);
        index = 8;
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
            Pick(1);
            troops[0].transform.position = spawnPoint.transform.position;
            troops[0].transform.rotation = spawnPoint.transform.rotation;
            state = State.Spawning;
        }
        else if (state == State.Dead)
        {
            Clear();
        }

        base.RightClick();
    }
}
