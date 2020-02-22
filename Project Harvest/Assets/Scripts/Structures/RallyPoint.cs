﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyPoint : Structure
{
    public Material red;
    public Material green;

    protected override void Start()
    {
        var fabric = transform.GetChild(0).transform.GetChild(0);
        var mr = fabric.GetComponent<SkinnedMeshRenderer>();
        mr.material = fruit ? red : green;
        base.Start();
    }

    public override void Remove()
    {
        ClearPointOnFarms();
        base.Remove();
    }

    private void ClearPointOnFarms()
    {
        var list = fruit ? Game.Instance.fruitStructures : Game.Instance.veggieStructures;
        foreach (Structure s in list)
        {
            if (s is Farm && (s as Farm).rallyPoint == transform.position)
                (s as Farm).rallyPoint = new Vector3();
        }
    }

    public void SetPointOnFarms()
    {
        var list = fruit ? Game.Instance.fruitStructures : Game.Instance.veggieStructures;
        foreach (Structure s in list)
        {
            if (s is Farm && (s as Farm).rallyPoint == new Vector3())
                (s as Farm).FindRallyPoint();
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}
