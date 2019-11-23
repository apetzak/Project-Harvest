using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TroopClass
{
    public string name;
    public int index;
    public bool fruit = true;
    public float speed;
    public float maxHealth;
    public float attackDamage;
    public float attackRange;
    public float attackSpeed;
    public float lineOfSight;

    public static List<TroopClass> GetProperties()
    {
        var list = new List<TroopClass>();
        list.Add(new TroopClass()
        {
            name = "Apple",
            index = 2,
            fruit = true,
            speed = 7,
            maxHealth = 150,
            attackDamage = 15,
            attackRange = 8,
            attackSpeed = 1.25f,
            lineOfSight = 30
        });
        list.Add(new TroopClass()
        {
            name = "Carrot",
            index = 3,
            fruit = false,
            speed = 13,
            maxHealth = 90,
            attackDamage = 30,
            attackRange = 15,
            attackSpeed = 1f,
            lineOfSight = 30
        });
        list.Add(new TroopClass()
        {
            name = "Banana",
            index = 4,
            fruit = true,
            speed = 6,
            maxHealth = 90,
            attackDamage = 10,
            attackRange = 40,
            attackSpeed = .5f,
            lineOfSight = 30
        });
        list.Add(new TroopClass()
        {
            name = "Brocolli",
            index = 5,
            fruit = false,
            speed = 5,
            maxHealth = 85,
            attackDamage = 6,
            attackRange = 30,
            attackSpeed = .25f,
            lineOfSight = 30
        });
        list.Add(new TroopClass()
        {
            name = "Pear",
            index = 6,
            fruit = true,
            speed = 5,
            maxHealth = 100,
            attackDamage = 40,
            attackRange = 90,
            attackSpeed = 4.5f,
            lineOfSight = 30
        });
        list.Add(new TroopClass()
        {
            name = "Corn",
            index = 7,
            fruit = false,
            speed = 3,
            maxHealth = 120,
            attackDamage = 50,
            attackRange = 100,
            attackSpeed = 5,
            lineOfSight = 30
        });
        list.Add(new TroopClass()
        {
            name = "Strawberry",
            index = 8,
            fruit = true,
            speed = 4,
            maxHealth = 120,
            attackDamage = 25,
            attackRange = 90,
            attackSpeed = 3,
            lineOfSight = 30
        });
        list.Add(new TroopClass()
        {
            name = "Asparagus",
            index = 9,
            fruit = false,
            speed = 4,
            maxHealth = 110,
            attackDamage = 30,
            attackRange = 110,
            attackSpeed = 4,
            lineOfSight = 30
        });
        return list;
    }
}

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
        if (deathTimer == 150)
        {
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

//public class Troops
//{
//    public static void SetGroupDestination(RaycastHit rh)
//    {

//    }
//}