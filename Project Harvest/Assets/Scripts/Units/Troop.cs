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
    public float timeUntilNextAttack;

    public void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined;
        timeUntilNextAttack = attackSpeed * 60;
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
                    Pursue();
                else
                    AttackStructure();
            }

            else if (attacking)
                FindClosestTarget();
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
    /// <param name="e"></param>
    public virtual void Attack(Entity e)
    {
        target = e;
        attacking = true;
        SetDestination(target.transform.position);
    }

     /// <summary>
     /// Chase targeted enemy unit
     /// </summary>
    private void Pursue()
    {
        destination = target.transform.position;
        Vector3 diff = transform.position - destination;
        moving = diff.magnitude >= attackRange;

        if (timeUntilNextAttack == 0 && !moving)
        {
            timeUntilNextAttack = attackSpeed * 60;
            //targetIndex = GetEnemyTroops().IndexOf(target);
            Audio.Instance.PlayAttack(index);
            TriggerAttack();
        }

        if ((target as Unit).moving)
        {
            velocity = GetVelocity(diff.x, diff.z);
            RotateTowards(diff.x, diff.z);
        }
    }


    private void AttackStructure()
    {

    }

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
            Attack(GetEnemyTroops()[index]);
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
                Attack(GetEnemyWorkers()[index]);
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
                    Attack(GetEnemyStructures()[index]);
                }
            }
        }

        if (closest == 10000)
        {
            attacking = false;
            StopMoving();
        }
    }

    public virtual void TriggerAttack()
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