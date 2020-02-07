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

    protected virtual void Start()
    {
        health = maxHealth;
    }

    protected virtual void Update()
    {
        if (isDying)
            Destroy(gameObject);
        else if (health <= 0)
            isDying = true;
    }

    protected virtual void LeftClick()
    {
        Debug.Log("left click");
        UnitUtils.ClearSelection();
        Game.Instance.ChangeSelection();
        Game.Instance.selectedUnit = this;
    }

    protected virtual void RightClick()
    {
        if (Game.Instance.troopIsSelected)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (!Physics.Raycast(ray, out hit))
                return;

            foreach (Troop t in Game.Instance.selectedUnits)
                t.TargetStructure(this, new Vector3(hit.point.x, t.transform.position.y, hit.point.z));
        }
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
