using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Resource
{
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {

    }

    protected override void LeftClick()
    {

    }

    void OnMouseEnter()
    {
        if (Game.Instance.workerIsSelected)
            CursorSwitcher.Instance.Set(8);
    }
}
