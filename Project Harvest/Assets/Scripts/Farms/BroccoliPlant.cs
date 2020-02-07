using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroccoliPlant : Farm
{
    public int plantGrowthTime = 16;
    public GameObject prop2;
    private int pickCount;

    public override void Start()
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

    public override void GrowProp()
    {
        prop.transform.localScale += new Vector3(5, 5, 5);
    }

    public override void Update()
    {
        if (state == FarmState.PlantGrowing) // grow plant first
        {
            plantGrowthTime--;
            prop2.transform.localScale += new Vector3(.25f, .25f, .25f);

            if (plantGrowthTime <= 0)
            {
                propMesh.enabled = true;
                state = FarmState.Growing;
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
        if (state == FarmState.Spawning)
            return;

        if (state == FarmState.Empty)
        {
            dirtMesh.enabled = true;
            state = FarmState.Planting;
        }
        else if (state == FarmState.Planting)
        {
            prop2.GetComponent<MeshRenderer>().enabled = true;
            state = FarmState.PlantGrowing;
        }
        else if (state == FarmState.Pickable)
        {
            Pick(1);
            propMesh.enabled = true;
            state = FarmState.Growing;
            pickCount--;
            if (pickCount <= 0) // stop growing troops
            {
                propMesh.enabled = false;
                System.Random rand = new System.Random();
                pickCount = rand.Next(2, 4);
                state = FarmState.Dead;
            }
            else // grow another troop
            {
                propMesh.enabled = true;
                prop.transform.localScale = new Vector3(0, 0, 0);
            }
        }
        else if (state == FarmState.Dead)
        {
            prop.transform.localScale = new Vector3(0, 0, 0);
            prop2.transform.localScale = new Vector3(0, 0, 0);
            prop2.GetComponent<MeshRenderer>().enabled = false;
            state = FarmState.Empty;
        }

        base.LeftClick();
    }
}
