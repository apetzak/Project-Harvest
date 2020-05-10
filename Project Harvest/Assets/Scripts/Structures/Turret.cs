using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Targets and fires at closest enemy unit within attack range
/// </summary>
public class Turret : Structure
{
    public GameObject gun;
    public Burst burst;
    private int attackTime = 60;
    public Entity target;
    private float facingAngle = 90;
    private float scanTime = 20;
    // todo: target closest or farthest

    protected override void Update()
    {
        if (!isBuilt)
            return;

        if (target == null)
        {
            scanTime--;

            // search for enemy every 20 ticks
            if (scanTime <= 0)
            {
                FindTarget();
                scanTime = 20;
            }
        }
        else
        {
            attackTime--;

            if (target is Unit && (target as Unit).moving)
            {
                Vector3 diff = transform.position - target.transform.position;

                if (diff.magnitude > 120) // unit left range
                {
                    scanTime = 20;
                    target = null;
                    return;
                }
                else
                {
                    RotateTowardsTarget();
                }
            }

            if (attackTime <= 0)
                ShootTarget();
        }
        base.Update();
    }

    private void ShootTarget()
    {
        // sniper shot
        AudioPlayer.Instance.PlaySound(TroopClass.Instance.list[5].sounds[0]);

        burst.Pop();
        attackTime = 60;
        target.health -= 50;

        if (target.health <= 0)
        {
            if (target is Unit)
                target.isDying = true;
            target.TakeDamage();
            target = null;
        }
    }

    private void FindTarget()
    {
        if (fruit)
        {
            foreach (Veggie v in Game.Instance.veggies)
            {
                if (TargetFound(v as Entity))
                    return;
            }
            foreach (Structure s in Game.Instance.veggieStructures)
            {
                if (TargetFound(s as Entity))
                    return;
            }
            foreach (Farm f in Game.Instance.veggieFarms)
            {
                if (TargetFound(f as Entity))
                    return;
            }
            foreach (Worker w in Game.Instance.peas)
            {
                if (TargetFound(w as Entity))
                    return;
            }
        }
        else
        {
            foreach (Fruit f in Game.Instance.fruits)
            {
                if (TargetFound(f as Entity))
                    return;
            }
            foreach (Structure s in Game.Instance.fruitStructures)
            {
                if (TargetFound(s as Entity))
                    return;
            }
            foreach (Farm f in Game.Instance.fruitFarms)
            {
                if (TargetFound(f as Entity))
                    return;
            }
            foreach (Worker w in Game.Instance.blueberries)
            {
                if (TargetFound(w as Entity))
                    return;
            }
        }
    }

    private bool TargetFound(Entity e)
    {
        if (e.isDying)
            return false;
        Vector3 diff = transform.position - e.transform.position;
        if (diff.magnitude < 120) // target found
        {
            target = e;
            RotateTowardsTarget();
            return true;
        }
        return false;
    }

    private void RotateTowardsTarget()
    {
        Vector3 diff = transform.position - target.transform.position;
        float angleDiff = GetAngle(diff.x, diff.z);
        gun.transform.Rotate(0, angleDiff, 0, Space.Self);
        facingAngle += angleDiff;
    }

    private float GetAngle(float x, float z)
    {
        return Mathf.Atan2(x, z) * (180.0f / Mathf.PI) - facingAngle;
    }
}
