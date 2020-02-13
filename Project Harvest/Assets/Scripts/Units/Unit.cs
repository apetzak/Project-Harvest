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
    protected float rotSpeed;
    public float currentSpeed;
    public float facingAngle;
    public float angleToRotate;
    public float lineOfSight;
    public int deathTimer = 300;
    private int clickTimer;
    private bool clickedOnce = false;
    public Entity target;
    private static float rad = 180.0f / Mathf.PI;
    protected Collider coll;
    protected Vector3 diff;

    /// <summary>
    /// Set collider
    /// </summary>
    protected virtual void Start()
    {
        coll = GetComponent<Collider>();
        rotSpeed = speed + 4;
    }

    protected virtual void Update()
    {
        if (isDying)
            Die();

        else if (health <= 0)
            isDying = true;

        else if (moving)
            Move();

        // bandaid for weird bug (colliders don't move with object)
        coll.enabled = false;
        coll.enabled = true;
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

    protected virtual void Move()
    {
        if (Mathf.Abs(angleToRotate) > 10) // rotating towards destination
        {
            float deg = angleToRotate > 0 ? rotSpeed : rotSpeed * -1;
            facingAngle += deg;
            transform.Rotate(0, deg, 0, Space.Self);
            angleToRotate -= deg;
        }
        else if (angleToRotate != 0) // finished rotating, start moving
        {
            facingAngle += angleToRotate;
            transform.Rotate(0, angleToRotate, 0, Space.Self);
            angleToRotate = 0;
            velocity = GetVelocity();
        }
    }

    public void SetDestination(Vector3 v)
    {
        currentSpeed = speed;
        destination = v;
        diff = transform.position - v;
        velocity = GetVelocity();
        angleToRotate = GetAngle();
        moving = true;
    }

    public Vector3 GetVelocity()
    {
        return new Vector3(-diff.x, 0, -diff.z).normalized;
    }

    public void StopMoving()
    {
        moving = false;
        velocity = new Vector3();
        destination = transform.position;
    }

    public float GetAngle()
    {
        float f = Mathf.Atan2(diff.x, diff.z) * rad - facingAngle;
        if (f > 180)
            f -= 360;
        else if (f < -180)      
            f += 360;
        return f;
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
            base.Remove();
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