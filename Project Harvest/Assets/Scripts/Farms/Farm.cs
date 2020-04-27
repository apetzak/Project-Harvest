using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns troops (each has associated subclass of farm).
/// Has various growth stages.
/// Built and maintained by workers.
/// </summary>
public class Farm : Structure
{
    public enum State
    {
        Grassy,
        Empty,
        Planting,
        Sprouting, // todo
        PlantGrowing,
        Growing,
        Pickable,
        Spawning,
        Dead,
        Decayed
    }

    public State state = State.Empty;
    /// <summary>
    /// Main object of the farm (plant, tree, bush, etc), grows during growing state
    /// </summary>
    public GameObject prop;
    public GameObject dirtMound;
    public MeshRenderer propMesh;
    public MeshRenderer dirtMesh;
    public List<MeshRenderer> grassMeshes = new List<MeshRenderer>();
    public List<Troop> troops;
    public int size;
    public int grassCount;
    public int growthTime = 0;
    /// <summary>
    /// Time it takes prop to fully grow
    /// </summary>
    public int growthEnd;
    public int spawnTime = 0;
    /// <summary>
    /// Time it takes for troop to spawn (reach normal size and position)
    /// </summary>
    public int spawnEnd;
    public int sproutTime = 0;
    public int decayTime;
    public int decayEnd = 3600;
    /// <summary>
    /// Time it takes plant to start growing after planted
    /// </summary>
    public int sproutEnd;
    public string troop;
    public Vector3 rallyPoint;
    public bool hasSprinkler;
    public bool isOccupied = false;

    /// <summary>
    /// Set/disable prop and dirt mesh
    /// </summary>
    protected override void Start()
    {
        propMesh = prop.GetComponent<MeshRenderer>();
        dirtMesh = dirtMound.GetComponent<MeshRenderer>();
        dirtMesh.enabled = propMesh.enabled = false;
        troop = GetTroopName();
        size = GetSize();
        grassCount = 300 * size;
        GetGrassMeshes();
        sproutEnd = 480;
        base.Start();
        // BananaTree doesn't call base.Start()
    }

    protected string GetTroopName()
    {
        string troopName = name;
        string[] types = { "(Clone)", "Tree", "Bush", "Plant", "Patch", "Stalk", "Vine" };
        foreach (string s in types)
        {
            if (name.Contains(s))
                troopName = troopName.Replace(s, "");
        }
        return troopName;
    }

    protected int GetSize()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "Patch1")
                return 1;
            else if (transform.GetChild(i).name == "Patch2")
                return 2;
            else if (transform.GetChild(i).name == "Patch3")
                return 3;
        }
        return 0;
    }

    protected void GetGrassMeshes()
    {
        Transform patch = transform.GetChild(1);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("Patch"))
            {
                patch = transform.GetChild(i);
                break;
            }
        }

        for (int i = 0; i < patch.childCount; i++)
        {
            if (patch.GetChild(i).name.Contains("GrassPatch"))
                grassMeshes.Add(patch.GetChild(i).GetComponent<MeshRenderer>());
        }
    }

    public void ShowGrass(bool show = true)
    {
        foreach (MeshRenderer mr in grassMeshes)
            mr.enabled = show;
        state = show ? State.Grassy : State.Empty;
        //Debug.Log(grassMeshes.Count);
    }

    public void SetDecayed()
    {
        //foreach (MeshRenderer mr in grassMeshes)
        //    mr.material = Game.Instance.grassDecayMat;
    }

    protected virtual void GrowProp()
    {

    }

    /// <summary>
    /// Grow crop if in growing state
    /// </summary>
    protected override void Update()
    {
        if (state == State.Sprouting)
        {
            sproutTime++;
            if (sproutTime == sproutEnd)
                StartGrowing();
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
                SetDecayed();
                state = State.Decayed;
            }
        }
        base.Update();
    }

    /// <summary>
    /// Retrieve troops from pickable farm
    /// </summary>
    /// <param name="count">Quantity to pick</param>
    /// <returns>Harvested troops</returns>
    public void Pick(int count)
    {
        //count += 5;
        troops.Clear();

        if (propMesh != null)
            propMesh.enabled = false;
        dirtMesh.enabled = false;

        Vector3 pos = prop.transform.position;

        for (int i = 0; i < count; i++)
        {
            Troop t = Instantiate(Assets.GetTroop(troop), pos, Quaternion.identity);

            // spread out
            if (count > 1)
                pos.x += 3;

            t.Spawn();
            troops.Add(t);
        }

        state = State.Empty;
    }

    /// <summary>
    /// Locate ally RallyPoint
    /// </summary>
    public void FindRallyPoint()
    {
        var list = fruit ? Game.Instance.fruitStructures : Game.Instance.veggieStructures;
        foreach (Structure s in list)
        {
            if (!(s is RallyPoint))
                continue;
            float dist = (transform.position - s.transform.position).magnitude;
            if (dist < 120)
            {
                rallyPoint = s.transform.position;
                break;
            }
        }
    }

    /// <summary>
    /// Set troops destination to rallyPoint, clear troops.
    /// </summary>
    public void MoveToRallyPoint()
    {
        if (rallyPoint == new Vector3())
            return;
        foreach (Troop t in troops)
        {
            Vector3 v = new Vector3(rallyPoint.x + Random.Range(-15, 15), rallyPoint.y, rallyPoint.z + Random.Range(-15, 15));
            t.SetDestination(v);
        }
        troops.Clear();
    }

    public virtual void StartPicking()
    {

    }

    public virtual void StartPlantGrowing()
    {

    }

    /// <summary>
    /// Show dirt pile, switch to planting state
    /// </summary>
    public void StartPlanting()
    {
        dirtMesh.enabled = true;
        state = State.Sprouting;
    }

    /// <summary>
    /// Show plant prop, switch to growing state
    /// </summary>
    public virtual void StartGrowing()
    {
        propMesh.enabled = true;
        state = State.Growing;
    }

    /// <summary>
    /// Show plant prop, switch to spawning state
    /// </summary>
    protected void StartSpawning()
    {
        if (propMesh != null)
            propMesh.enabled = true;
        state = State.Spawning;
    }

    /// <summary>
    /// Hide plant prop, switch to empty state
    /// </summary>
    protected void Clear()
    {
        propMesh.enabled = false;
        state = State.Empty;
    }

    /// <summary>
    /// Switch cursor
    /// </summary>
    protected override void RightClick()
    {
        CursorSwitcher.Instance.Switch(this);

        if (SendTroopsToAttack())
            return;

        if (state == State.Spawning || isOccupied || !Game.Instance.workerIsSelected)
            return;

        if (state == State.Grassy)
        {
            SendWorker(Worker.State.Raking);
        }
        else if (state == State.Pickable)
        {
            SendWorker(Worker.State.Picking);
        }
        else if (state == State.Dead && this is FruitTree)
        {
            SendWorker(Worker.State.ChoppingFruitTree);
        }
    }

    public void SendWorker(Worker.State state)
    {
        foreach (Unit u in Game.Instance.selectedUnits)
        {
            if (u.fruit == fruit && u is Worker)
            {
                (u as Worker).TargetFarm(this, state);
                return;
            }
        }
    }
}