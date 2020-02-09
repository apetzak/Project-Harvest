using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Produces units or provides upgrades/utility/defense.
/// Can be built or repaired by workers (using stone and wood).
/// Can be destroyed by troops.
/// </summary>
public class Structure : Entity
{
    public GameObject ring;
    public float minX;
    public float maxX;
    public float maxY;
    public float minY;
    public bool fruit;

    /// <summary>
    /// Set health to maxHealth
    /// </summary>
    protected virtual void Start()
    {
        health = maxHealth;
    }

    protected virtual void Update()
    {

    }

    /// <summary>
    /// Clear unit selection, set selected unit to this
    /// </summary>
    protected virtual void LeftClick()
    {
        if (Game.Instance.selectedUnit == this)
            return;

        ToggleRing();

        //Debug.Log("left click");
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

            foreach (Unit t in Game.Instance.selectedUnits)
            {
                if (t is Troop)
                    (t as Troop).TargetStructure(this, new Vector3(hit.point.x, t.transform.position.y, hit.point.z));
            }
        }
    }

    protected void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            LeftClick();
        else if (Input.GetMouseButtonDown(1))
            RightClick();
    }

    protected virtual void OnMouseEnter()
    {
        ToggleRing();

        if (Game.Instance.troopIsSelected)
            CursorSwitcher.Instance.Set(1);
        else if (Game.Instance.workerIsSelected)
            CursorSwitcher.Instance.Set(7);
    }

    private void OnMouseExit()
    {
        if (Game.Instance.selectedUnit != this)
            ToggleRing(false);
    }

    private void ToggleRing(bool b = true)
    {
        if (ring != null)
            ring.GetComponent<MeshRenderer>().enabled = b;
    }
}
