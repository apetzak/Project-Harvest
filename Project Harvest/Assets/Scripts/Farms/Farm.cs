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

            if (count > 1)
                pos.x += 3;

            Game.Instance.troops.Add(t);
            t.ToggleSelected(true);
            list.Add(t);
        }

        //Game.Instance.ChangeSelection(list.Count);

        state = FarmState.Empty;
        return list;
    }

    void OnMouseEnter()
    {
        CursorSwitcher.Instance.Set(8);
    }
}