using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Produces units or provides upgrades/utility/defense.
/// Can be built or repaired by workers.
/// Can be destroyed by troops.
/// </summary>
public class Structure : Entity
{
    public float minX;
    public float maxX;
    public float maxY;
    public float minY;

    void Start()
    {

    }

    protected virtual void LeftClick()
    {
        
    }

    protected virtual void RightClick()
    {

    }

    protected virtual void OnMouseEnter()
    {
        if (Game.Instance.troopIsSelected)
            CursorSwitcher.Instance.Set(1);
        else if (Game.Instance.workerIsSelected)
            CursorSwitcher.Instance.Set(7);
    }

    protected void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            LeftClick();
        else if (Input.GetMouseButtonDown(1))
            RightClick();
    }
}
