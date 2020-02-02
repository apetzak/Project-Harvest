using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTree : Farm
{
    public int plantGrowthTime = 200;
    private int pickCount = 3;
    public List<GameObject> props;
    public int harvestCount;
    public float plantGrowthIncrement;
    public float stopHeight;

    public override void Start()
    {
        ShowFruit(false);
        base.Start();
        propMesh.enabled = true;
        prop.transform.localScale = new Vector3(0, 0, 0);
        stopHeight = 11;
    }

    public override void Update()
    {
        if (state == FarmState.PlantGrowing) // grow tree
        {
            plantGrowthTime--;
            prop.transform.localScale += new Vector3(plantGrowthIncrement, plantGrowthIncrement, plantGrowthIncrement);

            if (plantGrowthTime <= 0)
            {
                dirtMesh.enabled = false;
                state = FarmState.Growing;
                plantGrowthTime = 200;
            }
        }
        else if (state == FarmState.Growing) // tree is grown, grow fruits
        {
            growthTime++;

            if (growthTime >= growthEnd)
            {
                ShowFruit();
                state = FarmState.Pickable;
                growthTime = 0;
            }
        }
        else if (state == FarmState.Spawning) // harvest has been picked, drop and grow troops
        {
            spawnTime--;

            for (int i = 0; i < harvestCount; i++)
            {
                troops[i].transform.localScale += new Vector3(1, 1, 1);

                if (troops[i].transform.position.y > stopHeight)
                    troops[i].transform.position -= new Vector3(0, .5f, 0);

                //Debug.Log(spawnTime + " " + (troops[i].transform.position.y > stopHeight)
                //    + " " + stopHeight + " " + transform.position.y);
            }

            if (spawnTime <= 0)
            {
                spawnTime = spawnStart;
                pickCount--;

                if (pickCount == 0)
                    state = FarmState.Dead;
                else
                    state = FarmState.Growing;
            }
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
            state = FarmState.PlantGrowing;
        }
        else if (state == FarmState.Pickable)
        {
            troops = Pick(harvestCount);
            if (propMesh != null)
                propMesh.enabled = true;

            for (int i = 0; i < harvestCount; i++)
            {
                troops[i].transform.position = props[i].transform.position;
                troops[i].transform.localScale = new Vector3(0, 0, 0);
            }

            ShowFruit(false);
            state = FarmState.Spawning;
        }
        else if (state == FarmState.Dead)
        {
            pickCount = 3;
            prop.transform.localScale = new Vector3(0, 0, 0);
            dirtMesh.enabled = false;
            state = FarmState.Empty;
        }
    }

    public void ShowFruit(bool b = true)
    {
        foreach (GameObject p in props)
            p.GetComponent<MeshRenderer>().enabled = b;
    }
}
