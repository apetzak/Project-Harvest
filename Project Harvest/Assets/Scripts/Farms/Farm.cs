using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Farm : Structure
{
    public enum FarmState
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

    public FarmState state = FarmState.Empty;
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

    /// <summary>
    /// Set/disable prop and dirt mesh, set spawnStart
    /// </summary>
    public virtual void Start()
    {
        propMesh = prop.GetComponent<MeshRenderer>();
        dirtMesh = dirtMound.GetComponent<MeshRenderer>();
        dirtMesh.enabled = propMesh.enabled = false;
        spawnStart = spawnTime;
    }

    public virtual void GrowProp()
    {

    }

    /// <summary>
    /// Grow crow if in growing state
    /// </summary>
    public virtual void Update()
    {
        if (state == FarmState.Growing)
        {
            growthTime++;
            GrowProp();

            if (growthTime >= growthEnd)
            {
                state = FarmState.Pickable;
                growthTime = 0;
            }
        }
    }

    /// <summary>
    /// Retrieve troops from pickable farm and select them
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public List<Troop> Pick(int count)
    {
        if (propMesh != null)
            propMesh.enabled = false;
        dirtMesh.enabled = false;

        var list = new List<Troop>();
        Vector3 pos = prop.transform.position;
        // todo: try to use Game.Instance.unitPrefabs[1]

        for (int i = 0; i < count; i++)
        {
            Troop t = Instantiate(prefab, pos, Quaternion.identity);

            // spread out
            if (count > 1)
                pos.x += 3;

            t.Spawn();
            t.ToggleSelected(true);
            list.Add(t);
        }

        Game.Instance.ChangeSelection();
        state = FarmState.Empty;
        return list;
    }

    protected override void OnMouseEnter()
    {
        if (Game.Instance.troopIsSelected)
            CursorSwitcher.Instance.Set(1);

        else if (Game.Instance.workerIsSelected)
            SwitchCursors();
    }

    /// <summary>
    /// Switch cursor, only if worker(s) are selected
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
        if (state == FarmState.Empty)
            CursorSwitcher.Instance.Set(2);

        else if (state == FarmState.Planting || state == FarmState.Growing 
              || state == FarmState.PlantGrowing || state == FarmState.Sprouting)
            CursorSwitcher.Instance.Set(4);

        else if (state == FarmState.Dead || state == FarmState.Pickable)
            CursorSwitcher.Instance.Set(5);
    }
}