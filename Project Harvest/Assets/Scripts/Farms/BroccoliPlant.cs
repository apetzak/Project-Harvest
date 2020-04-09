using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroccoliPlant : Farm
{
    public int plantGrowthTime;
    public GameObject prop2;
    private int pickCount;

    protected override void Start()
    {
        growthEnd = 1400;
        plantGrowthTime = 1600;
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
        prop.transform.localScale += new Vector3(.05f, .05f, .05f);
    }

    protected override void Update()
    {
        if (state == State.PlantGrowing) // grow plant first
        {
            plantGrowthTime--;
            prop2.transform.localScale += new Vector3(.0025f, .0025f, .0025f);

            if (plantGrowthTime <= 0)
            {
                propMesh.enabled = true;
                state = State.Growing;
            }
        }
        else if (state == State.Sprouting)
        {
            sproutTime++;
            if (sproutTime == sproutEnd)
                StartPlantGrowing();
        }
        else if (state == State.Growing)
        {
            growthTime++;
            GrowProp();

            if (growthTime >= growthEnd)
            {
                state = State.Pickable;
                growthTime = 0;
            }
        }
        else if (state == State.Dead)
        {
            decayTime++;
            if (decayTime >= decayEnd)
            {
                ShowGrass();
                state = State.Decayed;
            }
        }
    }

    public override void StartPlantGrowing()
    {
        prop2.GetComponent<MeshRenderer>().enabled = true;
        state = State.PlantGrowing;
    }

    public override void StartPicking()
    {
        Pick(1);
        MoveToRallyPoint();
        spawnTime = 0;
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

    protected override void RightClick()
    {
        //if (state == State.Dead)
        //{
        //    prop.transform.localScale = new Vector3(0, 0, 0);
        //    prop2.transform.localScale = new Vector3(0, 0, 0);
        //    prop2.GetComponent<MeshRenderer>().enabled = false;
        //    state = State.Empty;
        //}

        base.RightClick();
    }
}
