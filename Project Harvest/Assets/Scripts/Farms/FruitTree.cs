using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stages: Grow tree, grow troops numerous times, die, chop down
/// </summary>
public class FruitTree : Farm
{
    public int plantGrowthTime = 0;
    public int plantGrowthEnd = 200;
    private int pickCount = 3;
    public List<GameObject> props;
    public int harvestCount;
    public float plantGrowthIncrement;
    public float stopHeight;

    protected override void Start()
    {
        ShowFruit(false);
        base.Start();
        propMesh.enabled = true;
        prop.transform.localScale = new Vector3(0, 0, 0);
        stopHeight = 11;
    }

    protected override void Update()
    {
        if (state == State.PlantGrowing) // grow tree
        {
            plantGrowthTime++;
            prop.transform.localScale += new Vector3(plantGrowthIncrement, plantGrowthIncrement, plantGrowthIncrement);

            if (plantGrowthTime >= plantGrowthEnd)
            {
                dirtMesh.enabled = false;
                state = State.Growing;
                plantGrowthTime = 0;
            }
        }
        else if (state == State.Growing) // tree is grown, grow fruits
        {
            growthTime++;

            if (growthTime >= growthEnd)
            {
                ShowFruit();
                state = State.Pickable;
                growthTime = 0;
            }
        }
        else if (state == State.Spawning) // harvest has been picked, drop and grow troops
        {
            spawnTime++;

            for (int i = 0; i < harvestCount; i++)
            {
                troops[i].transform.localScale += new Vector3(.02f, .02f, .02f);

                if (troops[i].transform.position.y > stopHeight)
                    troops[i].transform.position -= new Vector3(0, .5f, 0);
            }

            if (spawnTime >= spawnEnd)
            {
                pickCount--;
                spawnTime = 0;
                MoveToRallyPoint();

                if (pickCount == 0)
                    state = State.Dead;
                else
                    state = State.Growing;
            }
        }

        if (isDying)
            Destroy(gameObject);
        else if (health <= 0)
            isDying = true;
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
            if (propMesh != null)
                propMesh.enabled = true;
            state = State.PlantGrowing;
        }
        else if (state == State.Pickable)
        {
            Pick(harvestCount);

            for (int i = 0; i < harvestCount; i++)
            {
                troops[i].transform.position = props[i].transform.position;
                troops[i].transform.localScale = new Vector3(0, 0, 0);
            }

            ShowFruit(false);
            StartSpawning();
        }
        else if (state == State.Dead)
        {
            pickCount = 3;
            prop.transform.localScale = new Vector3(0, 0, 0);
            dirtMesh.enabled = false;
            state = State.Empty;
        }

        base.LeftClick();
    }

    public void ShowFruit(bool b = true)
    {
        foreach (GameObject p in props)
            p.GetComponent<MeshRenderer>().enabled = b;
    }
}
