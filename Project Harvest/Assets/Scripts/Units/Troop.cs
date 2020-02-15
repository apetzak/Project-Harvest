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

    public AttackState attackState;
    public FocusState focusState;
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
            StopMoving();
        else if (!attacking || (attacking && diff.magnitude > attackRange))
            transform.Translate(velocity * currentSpeed / 10, Space.World);

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