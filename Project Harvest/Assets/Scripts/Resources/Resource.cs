using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Entity
{
    public bool occupied;
    public GameObject prop;
    public int stage = 0;
    public Worker.State workerstate;
    protected int cursorIndex = 0;

    /// <summary>
    /// Randomly change size by 25% plus or minus. Rotate random amount.
    /// Set maxHealth relative to new size
    /// </summary>
    protected virtual void Start()
    {
        float size = Random.Range(.75f, 1.25f);
        transform.Rotate(0, Random.Range(0, 360), 0);
        transform.localScale *= size;
        maxHealth = health = Mathf.RoundToInt(size * 100);
    }

    /// <summary>
    /// If worker is selected, switch worker state and set destination to this.position
    /// </summary>
    protected override void RightClick()
    {
        if (!Game.Instance.workerIsSelected)
            return;

        foreach (Unit u in Game.Instance.selectedUnits)
        {
            if (u is Worker)
            {
                (u as Worker).state = workerstate;

                if (occupied)
                    (u as Worker).FindNearestResource();
                else
                    (u as Worker).GatherFrom(this);
            }
        }
    }

    /// <summary>
    /// ClearSelection, ChangeSelection, set selectedEntity to this, toggle selectionChanged
    /// </summary>
    protected override void LeftClick()
    {
        EntityUtils.ClearSelection();
        Game.Instance.ChangeSelection();
        Game.Instance.selectedEntity = this;
        Game.Instance.selectionChanged = true;
    }

    /// <summary>
    /// Increment stage, shrink object, destroy at stage 5
    /// </summary>
    public void Shrink()
    { 
        if (health < maxHealth * .8f && stage == 0 ||
            health < maxHealth * .6f && stage == 1 ||
            health < maxHealth * .4f && stage == 2 ||
            health < maxHealth * .2f && stage == 3)
        {
            stage++;
            transform.localScale *= .87f;
        }
    }

    protected virtual void OnMouseEnter()
    {
        if (Game.Instance.workerIsSelected)
            CursorSwitcher.Instance.Set(cursorIndex);
    }

    /// <summary>
    /// Remove from resources, Destroy
    /// </summary>
    public override void Remove()
    {
        Game.Instance.resources.Remove(this);
        Destroy(gameObject);
    }
}
