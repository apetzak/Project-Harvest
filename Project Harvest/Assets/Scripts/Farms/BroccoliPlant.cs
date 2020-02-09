using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroccoliPlant : Farm
{
    public int plantGrowthTime = 16;
    public GameObject prop2;
    private int pickCount;

    protected override void Start()
    {
        growthEnd = 14;
        prop2.GetComponent<MeshRenderer>().enabled = false;
        prop2.transform.localScale = new Vector3(0, 0, 0);
        prop.transform.localScale = new Vector3(0, 0, 0);
        System.Random rand = new System.Random();
        pickCount = rand.Next(2, 4);
        index = 3;
        base.Start();
    }

    protected override void GrowProp()
    {
        prop.transform.localScale += new Vector3(5, 5, 5);
    }

    protected override void Update()
    {
        if (state == State.PlantGrowing) // grow plant first
        {
            plantGrowthTime--;
            prop2.transform.localScale += new Vector3(.25f, .25f, .25f);

            if (plantGrowthTime <= 0)
            {
                propMesh.enabled = true;
                state = State.Growing;
                plantGrowthTime = 16;
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
            prop2.GetComponent<MeshRenderer>().enabled = true;
            state = State.PlantGrowing;
        }
        else if (state == State.Pickable)
        {
            Pick(1);
            MoveToRallyPoint();
            spawnTime = spawnStart;
            StartGrowing();
            pickCount--;
            if (pickCount <= 0) // stop growing troops
            {
                propMesh.enabled = false;
                System.Random rand = new System.Random();
                pickCount = rand.Next(2, 4);
                state = State.Dead;
            }
            else // grow another troop
            {
                propMesh.enabled = true;
                prop.transform.localScale = new Vector3(0, 0, 0);
            }
        }
        else if (state == State.Dead)
        {
            prop.transform.localScale = new Vector3(0, 0, 0);
            prop2.transform.localScale = new Vector3(0, 0, 0);
            prop2.GetComponent<MeshRenderer>().enabled = false;
            state = State.Empty;
        }

        base.LeftClick();
    }
}
