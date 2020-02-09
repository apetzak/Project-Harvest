using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Structure
{
    public enum State
    {
        Empty,
        Planting,
        Sprouting, // todo
        PlantGrowing,
        Growing,
        Pickable,
        Spawning,
        Dead
    }

    public State state = State.Empty;
    public GameObject prop;
    public GameObject dirtMound;
    public MeshRenderer propMesh;
    public MeshRenderer dirtMesh;
    public Troop prefab;
    public List<Troop> troops;
    public int size;
    public int growthTime = 0;
    public int growthEnd;
    public int spawnTime;
    public int spawnStart;
    public int sproutTime = 30;
    public int index;
    public Vector3 rallyPoint;

    /// <summary>
    /// Set/disable prop and dirt mesh, set spawnStart
    /// </summary>
    protected override void Start()
    {
        propMesh = prop.GetComponent<MeshRenderer>();
        dirtMesh = dirtMound.GetComponent<MeshRenderer>();
        dirtMesh.enabled = propMesh.enabled = false;
        spawnStart = spawnTime;
        base.Start();
    }

    protected virtual void GrowProp()
    {

    }

    /// <summary>
    /// Grow crop if in growing state
    /// </summary>
    protected override void Update()
    {
        //if ((state == State.Empty || state == State.Dead) && rallyPoint != new Vector3())
        //{
        //    foreach (Troop t in troops)
        //        t.SetDestination(rallyPoint);
        //    rallyPoint = new Vector3();
        //}

        if (state == State.Growing)
        {
            growthTime++;
            GrowProp();

            if (growthTime >= growthEnd)
            {
                state = State.Pickable;
                growthTime = 0;
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
        troops.Clear();

        if (propMesh != null)
            propMesh.enabled = false;
        dirtMesh.enabled = false;

        Vector3 pos = prop.transform.position;
        // todo: try to use Game.Instance.unitPrefabs[1]

        for (int i = 0; i < count; i++)
        {
            Troop t = Instantiate(prefab, pos, Quaternion.identity);

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

    /// <summary>
    /// Show dirt pile, switch to planting state
    /// </summary>
    protected void StartPlanting()
    {
        dirtMesh.enabled = true;
        state = State.Planting;
    }

    /// <summary>
    /// Show plant prop, switch to growing state
    /// </summary>
    protected void StartGrowing()
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
    /// Set cursor (context sensitive)
    /// </summary>
    protected override void OnMouseEnter()
    {
        if (Game.Instance.troopIsSelected)
            CursorSwitcher.Instance.Set(1);

        else if (Game.Instance.workerIsSelected)
            SwitchCursors();
    }

    /// <summary>
    /// Start planting if state is empty.
    /// Switch cursor, if only worker(s) are selected
    /// </summary>
    protected override void LeftClick()
    {
        if (!Game.Instance.troopIsSelected && Game.Instance.workerIsSelected)
            SwitchCursors();
    }

    /// <summary>
    /// Set cursor icon to corresponding farm state
    /// </summary>
    protected void SwitchCursors()
    {
        if (state == State.Empty)
            CursorSwitcher.Instance.Set(2);

        else if (state == State.Planting || state == State.Growing 
              || state == State.PlantGrowing || state == State.Sprouting)
            CursorSwitcher.Instance.Set(4);

        else if (state == State.Dead || state == State.Pickable)
            CursorSwitcher.Instance.Set(5);
    }
}