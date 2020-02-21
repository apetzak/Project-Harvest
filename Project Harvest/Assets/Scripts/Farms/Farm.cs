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
    /// <summary>
    /// Main object of the farm (plant, tree, bush, etc), grows during growing state
    /// </summary>
    public GameObject prop;
    public GameObject dirtMound;
    public MeshRenderer propMesh;
    public MeshRenderer dirtMesh;
    public List<Troop> troops;
    public int size;
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
    /// <summary>
    /// Time it takes plant to start growing after planted
    /// </summary>
    public int sproutEnd;
    public int index;
    public Vector3 rallyPoint;

    /// <summary>
    /// Set/disable prop and dirt mesh
    /// </summary>
    protected override void Start()
    {
        propMesh = prop.GetComponent<MeshRenderer>();
        dirtMesh = dirtMound.GetComponent<MeshRenderer>();
        dirtMesh.enabled = propMesh.enabled = false;
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

        for (int i = 0; i < count; i++)
        {
            Troop t = Instantiate(Game.Instance.unitPrefabs[index], pos, Quaternion.identity);

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
    /// Switch cursor, if only worker(s) are selected
    /// </summary>
    protected override void RightClick()
    {
        if (!Game.Instance.troopIsSelected && Game.Instance.workerIsSelected)
            SwitchCursors();
        base.RightClick();
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