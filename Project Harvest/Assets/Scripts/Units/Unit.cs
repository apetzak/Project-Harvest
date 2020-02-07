using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Base class for troops and workers.
/// Can be selected, moved, and killed.
/// </summary>
public class Unit : Entity
{
    public GameObject selector;
    public int index;
    public Vector3 destination;
    public Vector3 velocity;
    public bool moving;
    public float speed;
    public float currentSpeed;
    public float facingAngle;
    public float lineOfSight;
    public int deathTimer = 300;
    private int clickTimer;
    private bool clickedOnce = false;
    public Entity target;
    private static float rad = 180.0f / Mathf.PI;
    private Collider coll;

    public void Start()
    {
        coll = GetComponent<Collider>();
    }

    protected virtual void Update()
    {
        if (isDying)
            Die();

        else if (health <= 0)
            isDying = true;

        else if (moving)
            Move();
    }

    public virtual void RightClick()
    {
        if (isDying)
            return;

        foreach (Troop t in GetEnemyTroops())
        {
            if (!t.selected)
                continue;
            t.TargetUnit(this);
        }
    }

    public virtual void SelectType()
    {

    }

    public virtual void OnMouseOver()
    {
        if (isDying)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            UnitUtils.ClearSelection();

            if (clickedOnce == true) // double click
            {
                SelectType();
            }
            else
            {
                ToggleSelected(true);
                clickedOnce = true;
            }
            Game.Instance.ChangeSelection();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RightClick();
        }

        if (clickedOnce)
            clickTimer++;

        if (clickTimer == 20) // cancel double click after 20
        {
            clickTimer = 0;
            clickedOnce = false;
        }
    }

    void OnMouseDown()
    {
        Game.Instance.holdingDown = false;
    }

    public void Move()
    {
        Vector3 v = transform.position - destination;

        if (Mathf.Abs(v.x) < 2 && Mathf.Abs(v.z) < 2)
            StopMoving();
        else
            transform.Translate(velocity * currentSpeed / 10, Space.World);

        // bandaid for weird bug (colliders don't move with object)
        coll.enabled = false;
        coll.enabled = true;
    }

    public void SetDestination(Vector3 v)
    {
        currentSpeed = speed;
        destination = v;
        Vector3 diff = transform.position - v;
        velocity = GetVelocity(diff.x, diff.z);
        RotateTowards(diff.x, diff.z);
        moving = true;
    }

    public Vector3 GetVelocity(float x, float z)
    {
        return new Vector3(-x, 0, -z).normalized;
    }

    public void StopMoving()
    {
        moving = false;
        velocity = new Vector3();
        destination = transform.position;
    }

    public virtual void RotateTowards(float x, float z)
    {
        float diff = GetAngle(x, z);
        transform.Rotate(0, diff, 0, Space.Self);
        facingAngle += diff;
    }

    public float GetAngle(float x, float z)
    {
        return Mathf.Atan2(x, z) * rad - facingAngle;
    }
 
    void OnMouseEnter()
    {
        if (Game.Instance.troopIsSelected && !isDying)
            CursorSwitcher.Instance.Set(1);
    }

    public void Die()
    {
        if (deathTimer == 300)
        {
            if (selected)
            {
                ToggleSelected(false);

                if (Game.Instance.selectedUnits.Contains(this))
                    Game.Instance.selectedUnits.Remove(this);

                Game.Instance.ChangeSelection();
            }

            Audio.Instance.PlayDeath(index);
            StopMoving();
            target = null;
        }

        deathTimer--;
        if (deathTimer > 210) // fall over
        {
            ToggleSelected(false);
            transform.Rotate(0, 0, 1, Space.World);
        }
        else if (deathTimer > 0) // sink
        {
            transform.Translate(0, -.1f, 0, Space.World);
        }
        else // delete
        {
            foreach (Troop t in GetEnemyTroops())
            {
                if (t.target != null && t.target == this)
                    t.target = null;
            }
            Destroy();
        }
    }

    public virtual List<Troop> GetEnemyTroops()
    {
        return null;
    }

    public virtual List<Unit> GetSameType()
    {
        return null;
    }

    public override void ToggleSelected(bool b)
    {
        //Debug.Log("toggle selected");

        var mr = selector.GetComponentInChildren<MeshRenderer>();
        mr.enabled = selected = b;

        if (b)
            Game.Instance.selectedUnits.Add(this);
    }
}