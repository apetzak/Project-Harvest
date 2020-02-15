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
    //public ParticleSystem fire;

    /// <summary>
    /// Set health to maxHealth
    /// </summary>
    protected virtual void Start()
    {
        health = maxHealth;
    }

    /// <summary>
    /// Empty
    /// </summary>
    protected virtual void Update()
    {

    }

    /// <summary>
    /// Clear unit selection, set selected unit to this
    /// </summary>
    protected override void LeftClick()
    {
        //UnitUtils.ClearSelection();
        //Game.Instance.ChangeSelection();
        //Game.Instance.selectedEntity = this;
        //ToggleRing();

        base.LeftClick();
    }

    /// <summary>
    /// TargetStructure() for all selected troops
    /// </summary>
    protected override void RightClick()
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
        if (Game.Instance.workerIsSelected)
        {
            foreach (Unit u in Game.Instance.selectedUnits)
            {
                if (u is Worker)
                    (u as Worker).SwitchState(Worker.State.Building);
            }
        }
    }

    /// <summary>
    /// Show ring, switch cursor
    /// </summary>
    protected virtual void OnMouseEnter()
    {
        ToggleRing();
        ToggleSelector();

        if (Game.Instance.troopIsSelected)
            CursorSwitcher.Instance.Set(1);
        else if (Game.Instance.workerIsSelected)
            CursorSwitcher.Instance.Set(7);
    }

    /// <summary>
    /// Toggle off ring if not selected
    /// </summary>
    private void OnMouseExit()
    {
        if (!selected)
        {
            ToggleRing(false);
            ToggleSelector(false);
        }
    }

    /// <summary>
    /// Show area ring
    /// </summary>
    /// <param name="b"></param>
    private void ToggleRing(bool b = true)
    {
        if (ring != null)
            ring.GetComponent<MeshRenderer>().enabled = b;
    }

    /// <summary>
    /// Show selector
    /// </summary>
    /// <param name="b"></param>
    private void ToggleSelector(bool b = true)
    {
        Debug.Log(selector +  " " + this.name);
        if (selector != null)
            selector.GetComponent<MeshRenderer>().enabled = b;
    }

    /// <summary>
    /// Play explosion, remove from collection, destroy
    /// </summary>
    public override void Remove()
    {
        Audio.Instance.PlayExplosion();

        if (fruit)
            Game.Instance.fruitStructures.Remove(this);
        else
            Game.Instance.veggieStructures.Remove(this);
       
        Destroy(gameObject);
    }

    /// <summary>
    /// Add to selectedStructures, enable/disable ring
    /// </summary>
    /// <param name="b"></param>
    public override void ToggleSelected(bool b)
    {
        if (b)
            Game.Instance.selectedStructures.Add(this);

        if (ring != null)
            ring.GetComponent<MeshRenderer>().enabled = b;

        if (selector != null)
            base.ToggleSelected(b);
        else
            selected = b;
    }

    public override void SelectType()
    {
        int selectedCount = 0;
        var list = fruit ? Game.Instance.fruitStructures : Game.Instance.veggieStructures;

        foreach (Structure s in list)
        {
            if (selectedCount > 84)
                break;

            if (s.fruit != fruit || s.GetType() != GetType())
                continue;

            selectedCount++;
            s.ToggleSelected(true);
            s.ToggleRing(true);
        }

        Game.Instance.ChangeSelection();
    }
}
