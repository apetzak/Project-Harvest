using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Troop : Unit
{
    public bool attacking;
    public float attackSpeed;
    public float attackRange;
    public float attackDamage;
    public float timeUntilNextAttack;
    public int targetIndex;

    public void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined;
        timeUntilNextAttack = attackSpeed * 60;
        health = maxHealth;
        //Debug.Log(burstMesh);
        //var v = new CapsuleCollider();
        base.Start();
    }

    //public Texture2D cursorTexture;
    //private CursorMode cursorMode = CursorMode.Auto;
    //private Vector2 hotSpot = Vector2.zero;

    //void OnMouseEnter()
    //{
    //    Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    //}

    //void OnMouseExit()
    //{
    //    Cursor.SetCursor(null, Vector2.zero, cursorMode);
    //}

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("collision");
    }

    public override void Update()
    {
        if (timeUntilNextAttack > 0)
            timeUntilNextAttack--;

        if (target != null && !target.dying)
            Pursue();

        else if (attacking)
            FindClosestTarget();

        if (dying)
            Die();

        else if (health <= 0)
            dying = true;

        else if (moving)
            Move();
    }

    public override void OnMouseOver()
    {
        //Debug.Log("troop");
        base.OnMouseOver();
        //var v = GetComponent<CapsuleCollider>();
        //Debug.Log(v.transform.position + " " + transform.position);
    }

    public override void RightClick()
    {
        foreach (Troop t in Game.Instance.troops)
        {
            if (!t.selected || fruit == t.fruit)
                continue;
            t.Attack(this);
        }
    }

    public virtual void Attack(Troop t)
    {
        target = t;
        attacking = true;
        SetDestination(target.transform.position);
        //u.Attack(this);
    }

    private void Pursue()
    {
        destination = target.transform.position;
        Vector3 diff = transform.position - destination;
        moving = diff.magnitude > attackRange;

        if (timeUntilNextAttack == 0 && !moving)
        {
            timeUntilNextAttack = attackSpeed * 60;
            targetIndex = Game.Instance.troops.IndexOf(target);
            Audio.Instance.PlayAttack(index);
            TriggerAttack();
        }

        if (target.moving)
            velocity = GetVelocity(diff.x, diff.z);

        RotateTowards(diff.x, diff.z);
    }

    private void FindClosestTarget()
    {
        float closest = 10000;
        int index = 0;

        foreach (Troop t in Game.Instance.troops)
        {
            if (fruit == t.fruit || t.dying)
                continue;
            Vector3 diff = transform.position - t.transform.position;

            if (diff.magnitude < closest)
            {
                closest = diff.magnitude;
                index = Game.Instance.troops.IndexOf(t);
            }
        }

        if (closest != 10000)
        {
            Attack(Game.Instance.troops[index]);
        }
        else
        {
            attacking = false;
            StopMoving();
        }
    }

    public void Die()
    {
        //Debug.Log(deathTimer);
        if (deathTimer == 150)
        {
            //Debug.Log("dead");
            Audio.Instance.PlayDeath(index);
            StopMoving();
            target = null;
        }

        deathTimer--;
        if (deathTimer > 60) // fall over
        {
            if (selected)
            {
                Game.Instance.selectionChanged = true;
                Game.Instance.selectionCount--;
            }
            ToggleSelected(false);
            transform.Rotate(0, 0, 1, Space.World);
        }
        else if (deathTimer > 0) // sink
        {
            transform.Translate(0, -.1f, 0, Space.World);
        }
        else // delete
        {
            foreach (Troop t in Game.Instance.troops)
            {
                if (t.target != null && t.target == this)
                    t.target = null;
            }

            Destroy(gameObject);
            Game.Instance.troops.Remove(this);
        }
    }

    public virtual void TriggerAttack()
    {
        if (target != null)
            target.health -= attackDamage;
    }
}