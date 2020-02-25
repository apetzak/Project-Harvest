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
        WoodCutting,
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
    public int resourceCount;
    public int resourceCapacity;
    public List<GameObject> tools;

    protected override void Start()
    {
        resourceCount = 0;
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
        else if (!moving)
        {
            if (state == State.WoodCutting)
            {
                if (resourceCount < resourceCapacity && target != null)
                    ChopTree();

                else if (!tools[9].activeSelf)
                    FindNearestDepository();

                else if (destination == transform.position) // arrived at depository
                    Game.Instance.fruitResourceWood += DepositLoad();
            }
            else if (state == State.GoldMining)
            {
                if (resourceCount < resourceCapacity && target != null)
                    MineOre();

                else if (!tools[9].activeSelf)
                    FindNearestDepository();

                else if (destination == transform.position)
                    Game.Instance.fruitResourceGold += DepositLoad();
            }
            else if (state == State.StoneMining)
            {
                if (resourceCount < resourceCapacity && target != null)
                    MineOre();

                else if (!tools[9].activeSelf)
                    FindNearestDepository();

                else if (destination == transform.position)
                    Game.Instance.fruitResourceStone += DepositLoad();
            }
            else if (state == State.Building)
            {
                SwingHammer();
            }
        }

        base.Update();
    }

    /// <summary>
    /// Hide sack, set resourceCount to 0, set destination to target or find nearest resource.
    /// </summary>
    /// <returns>The amount of resources deposited</returns>
    private int DepositLoad()
    {
        tools[9].SetActive(false); // un-sack
        int count = resourceCount;
        resourceCount = 0;
        if (target != null)
            SetDestination((target as Structure).GetWorkerDestination(this));
        else
            FindNearestResource();
        return count;
    }

    /// <summary>
    /// Hide all tools, show current tool, change state.
    /// If target is resource, set to occupied.
    /// </summary>
    /// <param name="s"></param>
    public void SwitchState(State s)
    {
        ResetTool((int)s);

        state = s;
        HideTools();

        if (s != State.Idle)
            ShowTool();

        if (target != null && (target is Resource))
            (target as Resource).occupied = false;

        if (resourceCount > 0)
            tools[9].SetActive(true);
    }

    /// <summary>
    /// Enable correct tool for current state, change animation time
    /// </summary>
    private void ShowTool()
    {
        animTime = 0;
        animEnd = 60;
        int toolIndex = 0;

        if (state == State.WoodCutting)
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
        //else if (s == State.) toolIndex = 7; // sickle
        else if (state == State.Waggoning)
            toolIndex = 8;
        else if (state == State.Sacking)
            toolIndex = 9;

        try
        {
            tools[toolIndex].SetActive(true);
        }
        catch
        {
            Debug.Log("tool " + toolIndex + " is broke");
        }

        //tools[toolIndex].SetActive(true);
    }

    /// <summary>
    /// Set all tools inactive
    /// </summary>
    private void HideTools()
    {
        foreach (GameObject o in tools)
            o.SetActive(false);
    }

    /// <summary>
    /// Once inventory is full: locate nearest repository, start sacking, SetDestination()
    /// </summary>
    private void FindNearestDepository()
    {
        Type t = state == State.WoodCutting ? typeof(LumberMill) :
                 state == State.CollectingWater ? typeof(WaterWell) : typeof(MiningCamp);

        float closest = 10000;
        int index = 0;
        foreach (Structure s in Game.Instance.fruitStructures)
        {
            if (s.GetType() == t)
            {
                float mag = (s.transform.position - transform.position).magnitude;

                if (mag < closest)
                {
                    closest = mag;
                    index = Game.Instance.fruitStructures.IndexOf(s);
                }
            }
        }

        if (closest != 10000)
        {
            tools[9].SetActive(true); // sacking
            SetDestination(Game.Instance.fruitStructures[index].GetWorkerDestination(this));
        }
        else
        {
            SwitchState(State.Idle);
        }
    }

    /// <summary>
    /// SwitchState, set target = r, set r.occupied, SetDestination
    /// </summary>
    /// <param name="r"></param>
    public void GatherFrom(Resource r)
    {
        SwitchState(r.workerstate);
        target = r;
        r.occupied = true;
        SetDestination(r.GetWorkerDestination(this)); // todo: set in front of resource
    }

    /// <summary>
    /// Locate and GatherFrom nearest unoccupied resource of current type
    /// </summary>
    public void FindNearestResource()
    {
        Type t = state == State.WoodCutting ? typeof(Tree) :
         state == State.GoldMining ? typeof(Gold) :
         state == State.StoneMining ? typeof(Stone) : typeof(Water);

        int index = 0;
        float lowestDistance = 10000;

        foreach (Resource r in Game.Instance.resources)
        {
            if (!r.occupied && r.GetType() == t && !r.isDying)
            {
                float f = (r.transform.position - transform.position).magnitude;
                if (f < lowestDistance)
                {
                    lowestDistance = f;
                    index = Game.Instance.resources.IndexOf(r);
                }
            }
        }

        if (lowestDistance == 10000)
        {
            if (resourceCount > 0)
            {
                tools[9].SetActive(true);
                FindNearestDepository();
            }
            else
            {
                SwitchState(State.Idle);
            }
        }
        else
        {
            GatherFrom(Game.Instance.resources[index]);
        }
    }

    private void FindNearestBuilding()
    {
        var list = fruit ? Game.Instance.fruitStructures : Game.Instance.veggieStructures;
        foreach (Structure s in list)
        {
            if (s.health < s.maxHealth && !(s is Farm))
            {
                target = s;
                SetDestination(s.GetWorkerDestination(this));
                return;
            }
        }
        SwitchState(State.Idle);
    }

    private void ChopTree()
    { 
        SwingTool(0, 0, 3, 0);

        if (animTime >= animEnd)
        {
            TakeFromResource();

            if (target.health <= 0)
            {
                (target as Tree).FallOver();
                SwitchResources();
            }
        }
    }

    private void MineOre()
    {
        SwingTool(1, 0, 0, 3);

        if (animTime >= animEnd)
        {
            TakeFromResource();
            (target as Resource).Shrink();

            if (target.health <= 0)
            {
                target.Remove();
                SwitchResources();
            }
        }
    }

    private void SwingHammer()
    {
        SwingTool(2, 0, 3, 0);

        if (animTime >= animEnd)
        {
            animTime = 0;

            if (target.health < target.maxHealth)
            {
                if (!(target as Structure).isBuilt)
                {
                    (target as Structure).Build();
                }
                else
                {
                    target.health += 10;
                    Game.Instance.fruitResourceWood -= 1;
                }
            }
            else
            {
                ResetTool(2);
                FindNearestBuilding();
            }
        }
    }

    private void SwingTool(int i, float x, float y, float z)
    {
        if (animTime < 30)
            tools[i].transform.Rotate(x, y, z);
        else
            tools[i].transform.Rotate(-x, -y, -z);
        animTime++;
    }

    private void ResetTool(int i)
    {

    }

    private void TakeFromResource()
    {
        resourceCount += 1;
        target.health -= 1;
        animTime = 0;
    }

    private void SwitchResources()
    {
        target = null;
        if (resourceCount == resourceCapacity)
            FindNearestDepository();
        else
            FindNearestResource();
    }

    private void CollectWater()
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
            {
                if (target != null)
                {
                    diff = transform.position - target.transform.position;
                    angleToRotate = GetAngle();
                    FaceTarget();
                }

                StopMoving();
            }
            else
            {
                transform.Translate(velocity * currentSpeed / 10, Space.World);
            }
        }
        else
        {
            base.Move();
        }
    }

    /// <summary>
    /// Drop down then walk to spawn point after landing
    /// </summary>
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
