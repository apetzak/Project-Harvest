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

    protected override void Update()
    {
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

    public override void OnMouseOver()
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
    }

    public virtual void TargetStructure(Entity e, Vector3 dest)
    {
        target = e;
        attacking = true;
        SetDestination(dest);
    }

    /// <summary>
    /// Chase targeted enemy unit
    /// </summary>
    private void PursueUnit()
    {
        destination = target.transform.position;
        Vector3 diff = transform.position - destination;
        moving = diff.magnitude >= attackRange;

        if (timeUntilNextAttack == 0 && !moving)
            TriggerAttack();

        if ((target as Unit).moving)
        {
            velocity = GetVelocity(diff.x, diff.z);
            RotateTowards(diff.x, diff.z);
        }
    }

    private void AttackStructure()
    {
        if (moving)
        {
            Vector3 diff = transform.position - destination;
            moving = diff.magnitude >= attackRange;
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

    /// <summary>
    /// Find and target closest enemy entity
    /// </summary>
    private void FindClosestTarget()
    {
        float closest = 10000;
        int index = 0;

        foreach (Troop t in GetEnemyTroops())
        {
            if (t.isDying)
                continue;
            Vector3 diff = transform.position - t.transform.position;

            if (diff.magnitude < closest)
            {
                closest = diff.magnitude;
                index = GetEnemyTroops().IndexOf(t);
            }
        }

        if (closest != 10000)
        {
            TargetUnit(GetEnemyTroops()[index]);
        }
        else
        {
            foreach (Worker w in GetEnemyWorkers())
            {
                if (w.isDying)
                    continue;
                Vector3 diff = transform.position - w.transform.position;

                if (diff.magnitude < closest)
                {
                    closest = diff.magnitude;
                    index = GetEnemyWorkers().IndexOf(w);
                }
            }

            if (closest != 10000)
            {
                TargetUnit(GetEnemyWorkers()[index]);
            }
            else
            {
                foreach (Structure s in GetEnemyStructures())
                {
                    if (s.isDying)
                        continue;
                    Vector3 diff = transform.position - s.transform.position;

                    if (diff.magnitude < closest)
                    {
                        closest = diff.magnitude;
                        index = GetEnemyStructures().IndexOf(s);
                    }
                }

                if (closest != 10000)
                {
                    TargetStructure(GetEnemyStructures()[index],
                    GetEnemyStructures()[index].transform.position);
                }
            }
        }

        if (closest == 10000)
        {
            attacking = false;
            StopMoving();
        }
    }

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