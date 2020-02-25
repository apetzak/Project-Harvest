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
    public int index;
    public Vector3 destination;
    public Vector3 velocity;
    public List<Vector3> movements;
    public bool moving;
    public float speed;
    protected float rotSpeed;
    public float currentSpeed;
    public float facingAngle;
    public float angleToRotate;
    public bool isUnderAttack;
    public float lineOfSight;
    public int deathTimer = 300;
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

    /// <summary>
    /// Die if dying or move if moving
    /// </summary>
    protected virtual void Update()
    {
        if (isDying)
            Die();

        else if (health <= 0)
            isDying = true;

        else if (moving)
            Move();
    }

    protected override void LeftClick()
    {
        base.LeftClick();
    }

    protected override void RightClick()
    {
        foreach (Troop t in GetEnemyTroops())
        {
            if (!t.selected)
                continue;
            t.TargetUnit(this);
        }
    }

    protected override void OnMouseOver()
    {
        base.OnMouseOver();
    }

    private void OnMouseDown()
    {
        Game.Instance.holdingDown = false;
    }

    /// <summary>
    /// Rotate towards destination then set moving velocity
    /// </summary>
    protected virtual void Move()
    {
        if (Mathf.Abs(angleToRotate) > 10) // rotating towards destination
        {
            float deg = angleToRotate > 0 ? rotSpeed : rotSpeed * -1;
            facingAngle += deg;
            transform.Rotate(0, deg, 0, Space.Self);
            angleToRotate -= deg;
        }
        else if (angleToRotate != 0) // finished rotatingd, start moving
        {
            FaceTarget();
            velocity = GetVelocity();
        }
    }

    protected void FaceTarget()
    {
        facingAngle += angleToRotate;
        transform.Rotate(0, angleToRotate, 0, Space.Self);
        angleToRotate = 0;
    }

    /// <summary>
    /// Set currentSpeed to normal, destination, diff, velocity, angleToRotate, and moving = true
    /// </summary>
    /// <param name="v"></param>
    public void SetDestination(Vector3 v)
    {
        currentSpeed = speed;
        destination = v;
        diff = transform.position - v;
        velocity = GetVelocity();
        angleToRotate = GetAngle();
        //CalculateMovements();
        moving = true;
    }

    public Vector3 GetVelocity()
    {
        return new Vector3(-diff.x, 0, -diff.z).normalized;
    }

    /// <summary>
    /// Set moving = false, velocity = new, desination = self
    /// </summary>
    public void StopMoving()
    {
        moving = false;
        velocity = new Vector3();
        destination = transform.position;
    }

    // todo: make this better
    public float GetAngle()
    {
        float f = Mathf.Atan2(diff.x, diff.z) * rad - facingAngle;
        if (f > 180)
            f -= 360;
        else if (f < -180)      
            f += 360;
        return f;
    }

    public void Die()
    {
        if (deathTimer == 300)
        {
            if (target is Resource)
                (target as Resource).occupied = false;

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
            Remove();
        }
    }

    public override void ToggleSelected(bool b)
    {
        if (b)
            Game.Instance.selectedUnits.Add(this);

        base.ToggleSelected(b);
    }

    private void CalculateMovements()
    {
        float mag = diff.magnitude;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, velocity, mag);

        if (hits.Length > 0)
        {
            foreach (RaycastHit h in hits)
            {
                Structure s = hits[0].transform.GetComponent<Structure>();
                if (s == null || s is Farm)
                    continue;

                Vector3 dest = new Vector3();
                bool isBelow = transform.position.x < s.transform.position.x;
                bool isLeft = transform.position.z < s.transform.position.z;

                int i = 5;

                if (isBelow)
                {
                    if (isLeft)
                        dest = new Vector3(s.maxX + i, 0, s.minZ - i);
                    else
                        dest = new Vector3(s.maxX + i, 0, s.maxZ + i);
                }
                else
                {
                    if (isLeft)
                        dest = new Vector3(s.minX + i, 0, s.minZ + i);
                    else
                        dest = new Vector3(s.minX - i, 0, s.minZ - i);
                }

                movements.Add(dest);
                //diff = transform.position - currentDestination;
                //velocity = GetVelocity();
                //angleToRotate = GetAngle();
                //Debug.Log(currentDestination);
                //return;
            }
            //Debug.Log(hits[0].transform.name + " " + hits[0].point + " " + bb.GetType());
        }
    }

    private void Explore()
    {

    }

    public virtual List<Troop> GetEnemyTroops()
    {
        return null;
    }

    public virtual List<Unit> GetSameType()
    {
        return null;
    }
}