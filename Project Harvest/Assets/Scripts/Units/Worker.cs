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
        GatheringWater,
        CarryingWater,
        Raking,
        Watering,
        GoldMining,
        StoneMining,
        WoodCutting,
        ChoppingFruitTree,
        Building,
        Planting,       
        Spawning,
        Digging,
        Waggoning,
        Sacking,
        Picking
    }

    public State state;
    public int animTime;
    public int animEnd;
    public int collectingTime;
    public int collectingEnd;
    public int resourceCount;
    public int resourceCapacity;
    public string sackResource;
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
        else if (!moving && state != State.Idle)
        {
            switch (state)
            {
                case State.Raking:
                    Rake();
                    break;
                case State.Planting:
                    PlantSeed();
                    break;
                case State.Watering:
                    WaterPlant();
                    break;
                case State.Picking:
                    PickPlant();
                    break;
                case State.ChoppingFruitTree:
                    PickPlant();
                    break;
                case State.Building:
                    BuildStructure();
                    break;
                case State.WoodCutting:
                    {
                        if (resourceCount < resourceCapacity && target != null)
                            ChopTree();

                        else if (!tools[9].activeSelf)
                            FindNearestDepository();

                        else if (destination == transform.position) // arrived at depository
                            AddToResources("wood");
                    }
                    break;
                case State.GoldMining:
                    {
                        if (resourceCount < resourceCapacity && target != null)
                            MineOre();

                        else if (!tools[9].activeSelf)
                            FindNearestDepository();

                        else if (destination == transform.position)
                            AddToResources("gold");
                    }
                    break;
                case State.StoneMining:
                    {
                        if (resourceCount < resourceCapacity && target != null)
                            MineOre();

                        else if (!tools[9].activeSelf)
                            FindNearestDepository();

                        else if (destination == transform.position)
                            AddToResources("stone");
                    }
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
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
            SetDestination((target as Structure).GetUnitDestination(this));
        else
            FindNearestResource();
        return count;
    }

    private void AddToResources(string resource)
    {
        if (fruit)
        {
            if (resource == "wood")
                Game.Instance.fruitResourceWood += DepositLoad();
            else if (resource == "stone")
                Game.Instance.fruitResourceStone += DepositLoad();
            else if (resource == "gold")
                Game.Instance.fruitResourceGold += DepositLoad();
        }
        else
        {
            if (resource == "wood")
                Game.Instance.veggieResourceWood += DepositLoad();
            else if (resource == "stone")
                Game.Instance.veggieResourceStone += DepositLoad();
            else if (resource == "gold")
                Game.Instance.veggieResourceGold += DepositLoad();
        }
    }

    /// <summary>
    /// Hide all tools, show current tool, change state.
    /// If target is resource, set to occupied.
    /// </summary>
    public void SwitchState(State s)
    {
        ResetTool();

        state = s;
        HideTools();

        if (s != State.Idle)
            ShowTool();

        if (target != null && (target is Resource))
        {
            (target as Resource).occupied = false;
            target = null;
        }

        if (slot != null && s != State.Building)
        {
            slot.occupied = false;
            slot = null;
        }

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

        if (state == State.WoodCutting || state == State.ChoppingFruitTree)
            toolIndex = 0;
        else if (state == State.GoldMining || state == State.StoneMining)
            toolIndex = 1;
        else if (state == State.Building)
            toolIndex = 2;
        else if (state == State.CarryingWater || state == State.GatheringWater)
            toolIndex = 3;
        else if (state == State.Watering)
        {
            toolIndex = 4;
            animEnd = 240;
        }
        else if (state == State.Raking)
            toolIndex = 5;
        else if (state == State.Digging || state == State.Planting || state == State.Picking)
            toolIndex = 6;
        //else if (s == State.) toolIndex = 7; // sickle
        else if (state == State.Waggoning)
            toolIndex = 8;
        else if (state == State.Sacking)
            toolIndex = 9;

        tools[toolIndex].SetActive(true);

        //try
        //{
        //    tools[toolIndex].SetActive(true);
        //}
        //catch
        //{
        //    Debug.Log("tool " + toolIndex + " is broke");
        //}
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
                 state == State.GatheringWater ? typeof(WaterWell) : typeof(MiningCamp);

        float closest = 100000;
        int index = -1;
        var structs = fruit ? Game.Instance.fruitStructures : Game.Instance.veggieStructures;
        foreach (Structure s in structs)
        {
            if (s.GetType() == t && s.isBuilt)
            {
                float mag = (s.transform.position - transform.position).sqrMagnitude;

                if (mag < closest)
                {
                    closest = mag;
                    index = structs.IndexOf(s);
                }
            }
        }

        if (index != -1)
        {
            tools[9].SetActive(true); // sacking
            if (fruit)
                SetDestination(Game.Instance.fruitStructures[index].GetUnitDestination(this));
            else
                SetDestination(Game.Instance.veggieStructures[index].GetUnitDestination(this));
        }
        else
        {
            SwitchState(State.Idle);
        }
    }

    /// <summary>
    /// SwitchState, set target = r, set r.occupied, SetDestination
    /// </summary>
    public void GatherFrom(Resource r)
    {
        SwitchState(r.workerState);
        target = r;
        r.occupied = true;
        SetDestination(r.GetUnitDestination(this));
    }

    /// <summary>
    /// Locate and GatherFrom nearest unoccupied resource of current type
    /// </summary>
    public void FindNearestResource()
    {
        Type t = state == State.WoodCutting ? typeof(Tree) :
         state == State.GoldMining ? typeof(Gold) :
         state == State.StoneMining ? typeof(Stone) : typeof(Water);

        int index = -1;
        float lowestDistance = 100000;

        foreach (Resource r in Game.Instance.resources)
        {
            if (!r.occupied && r.GetType() == t && !r.isDying)
            {
                float f = (r.transform.position - transform.position).sqrMagnitude;
                if (f < lowestDistance)
                {
                    lowestDistance = f;
                    index = Game.Instance.resources.IndexOf(r);
                }
            }
        }

        if (index != -1)
        {
            GatherFrom(Game.Instance.resources[index]);
        }
        else
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
    }

    public void FindNearestBuilding()
    {
        var list = fruit ? Game.Instance.fruitStructures : Game.Instance.veggieStructures;
        foreach (Structure s in list)
        {
            if (s.health < s.maxHealth && s.HasOpenSpot())
            {
                TargetStructure(s);
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

    private void BuildStructure()
    {
        if (target != null && target.health == target.maxHealth)
            target = null;
        
        if (target == null)
        {
            ResetTool();
            FindNearestBuilding();
            return;
        }

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

    private void ThrustTool(int i, float x, float y, float z)
    {
        if (animTime < 30)
            tools[i].transform.Translate(x, y, z);
        else
            tools[i].transform.Translate(-x, -y, -z);
        animTime++;
    }

    /// <summary>
    /// Finish animation so tool returns to default position
    /// </summary>
    private void ResetTool()
    {
        if (animTime > 0)
        {
            while (animTime < animEnd)
            {
                if (state == State.WoodCutting)
                    SwingTool(0, 0, 3, 0);
                if (state == State.GoldMining || state == State.StoneMining)
                    SwingTool(1, 0, 0, 3);
                else if (state == State.Building)
                    SwingTool(2, 0, 3, 0);
                else if (state == State.Raking)
                    ThrustTool(5, .1f, 0, 0);
                else
                    break;
            }
        }
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
        // todo
    }

    private void Rake()
    {
        ThrustTool(5, .1f, 0, 0);

        if (animTime >= animEnd)
            animTime = 0;

        (target as Farm).grassCount--;
        if ((target as Farm).grassCount == 0)
        {
            ResetTool();
            (target as Farm).ShowGrass(false);
            SetDestination(target.transform.position);
            SwitchState(State.Planting);
        }
        animTime++;
    }

    private void PlantSeed()
    {
        if (animTime < 30)
        {
            tools[6].transform.Translate(0, 0, -.1f, Space.Self);
        }
        else
        {
            tools[6].transform.Translate(0, 0, .1f, Space.Self);
            if (animTime == animEnd)
            {
                animTime = 0;
                (target as Farm).StartPlanting();
                SwitchState(State.Watering);
            }
        }
        animTime++;
    }

    private void PickPlant()
    {
        if (animTime < 30)
        {
            tools[6].transform.Translate(0, 0, -.1f, Space.Self);
        }
        else
        {
            tools[6].transform.Translate(0, 0, .1f, Space.Self);
            if (animTime == 60)
            {
                animTime = 0;
                (target as Farm).isOccupied = false;
                (target as Farm).StartPicking();
                InteractWithNearestFarm();
            }
        }
        animTime++;
    }

    private void WaterPlant()
    {
        if (animTime < 30)
        {
            tools[4].transform.Rotate(0, 0, 1f, Space.Self);
        }
        else if (animTime == 240)
        {
            animTime = 0;
            Farm f = target as Farm;
            f.state = Farm.State.Sprouting;
            f.isOccupied = false;
            InteractWithNearestFarm();
            return;
        }
        else if (animTime > 210)
        {
            tools[4].transform.Rotate(0, 0, -1f, Space.Self);
        }
        animTime++;
    }

    /// <summary>
    /// Find nearest unoccupied farm, rake or pick if possible
    /// </summary>
    public void InteractWithNearestFarm()
    {
        var farms = fruit ? Game.Instance.fruitFarms : Game.Instance.veggieFarms;
        foreach (Farm f in farms)
        {
            if (f.isOccupied)
                continue;
            if (f.state == Farm.State.Grassy)
            {
                TargetFarm(f, State.Raking);
                return;
            }
            if (f.state == Farm.State.Pickable)
            {
                TargetFarm(f, State.Picking);
                return;
            }
        }
        SwitchState(State.Idle);
    }

    public void TargetFarm(Farm f, State s)
    {
        target = f;
        f.isOccupied = true;
        SetDestination(f.GetUnitDestination(this));
        SwitchState(s);
    }

    public void TargetStructure(Structure s)
    {
        s.SetOpenSlot(this);
        if (slot == null) // no open slots
            return;
        SwitchState(Worker.State.Building);
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
