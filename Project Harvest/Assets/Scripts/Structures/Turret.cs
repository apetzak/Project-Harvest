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

    protected override void Update()
    {
        if (!isPlaced)
            return;

        if (target == null)
        {
            FindTarget();
        }
        else
        {
            attackTime--;

            if (target is Unit && (target as Unit).moving)
            {
                Vector3 diff = transform.position - target.transform.position;

                if (diff.magnitude > 120) // unit left range
                {
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
        Audio.Instance.PlaySound(TroopClass.Instance.list[5].sounds[0]);

        burst.Pop();
        attackTime = 60;
        target.health -= 50;

        if (target.health <= 0)
        {
            target.isDying = true;
            target = null;
        }
    }

    private void FindTarget()
    {
        if (fruit)
        {
            foreach (Veggie v in Game.Instance.veggies)
            {
                if (v.isDying)
                    continue;
                Vector3 diff = transform.position - v.transform.position;
                if (diff.magnitude < 120) // target found
                {
                    target = v;
                    RotateTowardsTarget();
                    break;
                }
            }
        }
        else
        {
            foreach (Fruit f in Game.Instance.fruits)
            {
                if (f.isDying)
                    continue;
                Vector3 diff = transform.position - f.transform.position;
                if (diff.magnitude < 120) // target found
                {
                    target = f;
                    RotateTowardsTarget();
                    break;
                }
            }
        }
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
