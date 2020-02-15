using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Entity
{
    public bool occupied;
    public GameObject prop;
    public int stage = 0;
    protected Worker.State workerstate;
    protected int cursorIndex = 0;

    protected virtual void Start()
    {
        float size = Random.Range(.75f, 1.25f);
        transform.Rotate(0, Random.Range(0, 360), 0);
        transform.localScale *= size;
        maxHealth = health = size * 1000;
    }

    protected override void RightClick()
    {
        foreach (Unit u in Game.Instance.selectedUnits)
        {
            if (u is Worker)
            {
                (u as Worker).SwitchState(workerstate);
                u.SetDestination(transform.position);
            }
        }
    }

    protected override void LeftClick()
    {

    }

    public void GatherFrom()
    {

    }

    /// <summary>
    /// Increment stage, shrink object, destroy at stage 5
    /// </summary>
    protected void Shrink()
    {
        stage++;
        transform.localScale *= .85f;

        if (stage == 5)
            Destroy(gameObject);
    }


    protected virtual void OnMouseEnter()
    {
        if (Game.Instance.workerIsSelected)
            CursorSwitcher.Instance.Set(cursorIndex);
    }
}
