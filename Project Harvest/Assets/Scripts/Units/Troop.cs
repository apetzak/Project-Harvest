using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Attacks enemy units or structures.
/// Each type has unique attributes, animations, and sounds.
/// Each type has corresponding farm, which they are grown from.
/// </summary>
public class Troop : Unit
{
    public enum AttackState
    {
        Agressive,
        Defensive,
        Passive,
    }

    public enum FocusState
    {
        Troops,
        Workers,
        Structures
    }

    public AttackState attackState; // todo
    public FocusState focusState; // todo
    public bool attacking;
    public float attackSpeed;
    public float attackRange;
    public float attackDamage;
    public float timeUntilNextAttack = 0;

    protected override void Start()
    {
        health = maxHealth;
        deathTimer = 300;
        base.Start();
    }

    private void Formate()
    {
        foreach (Troop t in Game.Instance.fruits)
        {
            if (t == this)
                continue;

            Vector3 diff = t.destination - destination;

            float dist = 1f;

            if (diff.magnitude < 2)
            {
                if (Mathf.Abs(destination.x - t.destination.x) < 1.5f)
                {
                    if (destination.x > t.destination.x)
                    {
                        destination += new Vector3(dist, 0, 0);
                        t.destination += new Vector3(-dist, 0, 0);
                    }
                    else
                    {
                        destination += new Vector3(-dist, 0, 0);
                        t.destination += new Vector3(dist, 0, 0);
                    }
                }

                if (Mathf.Abs(destination.z - t.destination.z) < 1.5f)
                {
                    if (destination.z > t.destination.z)
                    {
                        destination += new Vector3(0, 0, dist);
                        t.destination += new Vector3(0, 0, -dist);
                    }
                    else
                    {
                        destination += new Vector3(0, 0, -dist);
                        t.destination += new Vector3(0, 0, dist);
                    }
                }
            }
        }
    }

    protected override void Update()
    {
        //Formate();

        diff = transform.position - destination;

        if (!isDying)
        {
            if (timeUntilNextAttack > 0)
                timeUntilNextAttack--;

            if (target != null && !target.isDying)
            {
                if (target is Unit)
                    PursueUnit();
                else
                    AttackStructure();
            }
            else if (attacking)
            {
                FindClosestTarget();
            }
        }

        base.Update();
    }

    protected override void Move()
    {
        if (Mathf.Abs(diff.x) < 2 && Mathf.Abs(diff.z) < 2)
        {
            SetDestination(destination);
        }
        else if (!attacking || (attacking && diff.magnitude > attackRange))
        {
            transform.Translate(velocity * currentSpeed / 10, Space.World);
        }
        base.Move();
    }

    protected override void OnMouseOver()
    {
        base.OnMouseOver();
    }

    /// <summary>
    /// Set target, switch to attack mode, set destination
    /// </summary>
    /// <param name="e">Target to attack</param>
    public virtual void TargetUnit(Entity e)
    {
        target = e;
        attacking = true;
        SetDestination(target.transform.position);
        (e as Unit).isUnderAttack = true;
    }

    public virtual void TargetStructure(Structure s)
    {
        attacking = true;

        if (s is Farm || s is Hub)
        {
            target = s;
            SetDestination(s.transform.position);
            return;
        }

        var slot = s.GetOpenSlotLocation(this);
        if (slot == new Vector3())
        {
            FindClosestTarget();
        }
        else
        {
            target = s;
            SetDestination(slot);
        }
    }

    /// <summary>
    /// Chase targeted enemy unit
    /// </summary>
    private void PursueUnit()
    {
        if ((target as Unit).moving)
        {
            destination = target.transform.position;
            angleToRotate = GetAngle();
            velocity = GetVelocity();
        }

        moving = angleToRotate != 0 || diff.magnitude >= attackRange;

        if (timeUntilNextAttack == 0 && Mathf.Abs(angleToRotate) < 2 && diff.magnitude < attackRange)
            TriggerAttack();
    }

    private void AttackStructure()
    {
        if (moving)
        {
            moving = angleToRotate != 0 || diff.magnitude >= attackRange;
        }
        else if (timeUntilNextAttack == 0)
        {
            TriggerAttack();
            (target as Structure).TakeDamage();
        }
    }

    private void TriggerAttack()
    {
        timeUntilNextAttack = attackSpeed * 60;
        Audio.Instance.PlayAttack(index);
        InflictDamage();
    }

    #region Target Finding

    /// <summary>
    /// Find and target closest enemy entity
    /// </summary>
    public void FindClosestTarget()
    {
        float closest = 100000;

        int i = FindClosestEnemyTroop(ref closest);
        if (closest != 100000)
        {
            TargetUnit(GetEnemyTroops()[i]);
            return;
        }

        int i2 = FindClosestEnemyWorker(ref closest);
        if (closest != 100000)
        {
            TargetUnit(GetEnemyWorkers()[i2]);
            return;
        }

        int i3 = FindClosestEnemyStructure(ref closest);
        if (closest != 100000)
        {
            TargetStructure(GetEnemyStructures()[i3]);
            return;
        }

        if (closest == 100000)
        {
            attacking = false;
            StopMoving();
        }
    }

    private int FindClosestEnemyTroop(ref float closest)
    {
        int index = -1;
        foreach (Troop t in GetEnemyTroops())
        {
            if (t.isDying)
                continue;
            float dist = (transform.position - t.transform.position).sqrMagnitude;
            if (dist < closest)
            {
                if (t.isUnderAttack && UnityEngine.Random.Range(0, 2) > 1)
                    continue;
                closest = dist;
                index = GetEnemyTroops().IndexOf(t);
            }
        }
        return index;
    }

    private int FindClosestEnemyWorker(ref float closest)
    {
        int index = -1;
        foreach (Worker w in GetEnemyWorkers())
        {
            if (w.isDying)
                continue;
            float dist = (transform.position - w.transform.position).sqrMagnitude;
            if (dist < closest)
            {
                closest = dist;
                index = GetEnemyWorkers().IndexOf(w);
            }
        }
        return index;
    }

    private int FindClosestEnemyStructure(ref float closest)
    {
        int index = -1;
        foreach (Structure s in GetEnemyStructures())
        {
            if (s.isDying || !s.HasOpenSpot())
                continue;
            float dist = (transform.position - s.transform.position).sqrMagnitude;
            if (dist < closest)
            {
                closest = dist;
                index = GetEnemyStructures().IndexOf(s);
            }
        }
        return index;
    }

    #endregion

    /// <summary>
    /// Reduce targets health by attackDamage
    /// </summary>
    public virtual void InflictDamage()
    {
        if (target != null)
            target.health -= attackDamage;
    }

    public virtual List<Troop> GetAllyTroops()
    {
        return null;
    }

    public virtual List<Worker> GetEnemyWorkers()
    {
        return null;
    }

    public virtual List<Structure> GetEnemyStructures()
    {
        return null;
    }
}