using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit
{
    public enum State
    {
        Idle,
        Walking,
        CollectingWater,
        CarryingWater,
        Raking,
        Watering,
        GoldMining,
        StoneMining,
        Chopping,
        Building,
        Planting,       
        Spawning,
        Digging,
        Waggoning,
        Sacking
    }

    public State state;
    public int animTime;
    public int animEnd;
    public int collectingTime;
    public int collectingEnd;
    public Resource resource;
    public int resourceCount;
    public int resourceCapacity;
    public List<GameObject> tools;

    protected override void Start()
    {
        resourceCapacity = 10;
        state = State.Spawning;
        base.Start();
    }

    protected override void Update()
    {
        diff = transform.position - destination;

        if (state == State.Spawning)
        {
            MoveToSpawnPoint();
            return;
        }
        else if (state == State.Chopping)
        {
            if (moving)
            {

            }
            else
            {
                ChopTree();
            }
        }
        else if (state == State.GoldMining)
        {
            
        }
        else if (state == State.StoneMining)
        {

        }

        base.Update();
    }

    /// <summary>
    /// Hide all tools, show current tool, change state
    /// </summary>
    /// <param name="s"></param>
    public void SwitchState(State s)
    {
        //Debug.Log(s.ToString());
        state = s;
        HideTools();
        if (s != State.Idle)
            ShowTool();
    }

    private void ShowTool()
    {
        int toolIndex = 0;

        if (state == State.Chopping)
            toolIndex = 0;
        else if (state == State.GoldMining || state == State.StoneMining)
            toolIndex = 1;
        else if (state == State.Building)
            toolIndex = 2;
        else if (state == State.CarryingWater || state == State.CollectingWater)
            toolIndex = 3;
        else if (state == State.Watering)
            toolIndex = 4;
        else if (state == State.Raking)
            toolIndex = 5;
        else if (state == State.Digging)
            toolIndex = 6;
        //else if (s == State.)
        //    toolIndex = 7; // sickle
        else if (state == State.Waggoning)
            toolIndex = 8;
        else if (state == State.Sacking)
            toolIndex = 9;

        tools[toolIndex].SetActive(true);
    }

    private void HideTools()
    {
        foreach (GameObject o in tools)
            o.SetActive(false);
    }

    private void ChopTree()
    {
        
    }

    private void MineOre()
    {

    }

    private void CollectWater()
    {

    }

    private void BuiltStructure()
    {

    }

    private void RakePatch()
    {

    }

    private void PlantSeed()
    {

    }

    private void WaterPlant()
    {

    }
    
    private void ClearPatch()
    {

    }

    private void DumpFertilizer()
    {

    }

    protected override void Move()
    {
        if (angleToRotate == 0) // facing destination, move forward
        {
            if (Mathf.Abs(diff.x) < 2 && Mathf.Abs(diff.z) < 2)
                StopMoving();
            else
                transform.Translate(velocity * currentSpeed / 10, Space.World);
        }
        else
        {
            base.Move();
        }
    }

    private void MoveToSpawnPoint()
    {
        if (transform.position.y > 11) // drop
        {
            transform.Translate(0, -.5f, 0);

            if (transform.position.y <= 11) // set on ground
                transform.position = new Vector3(transform.position.x, 11, transform.position.z);
        }
        else if (moving) // move towards destination
        {
            Move();
        }
        else // stop
        {
            state = State.Idle;
        }
    }
}
