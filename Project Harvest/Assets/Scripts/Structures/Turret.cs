using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Structure
{
    public GameObject gun;
    public Burst burst;
    private int attackTime = 60;
    public bool attacking = false;
    public Entity target;
    private float facingAngle = 90;

    protected override void Update()
    {
        if (target == null)
        {
            FindTarget();
        }
        else if (attacking)
        {
            attackTime--;

            if (target is Unit && (target as Unit).moving)
            {
                Vector3 diff = transform.position - target.transform.position;

                if (diff.magnitude > 60) // unit left range
                {
                    target = null;
                    attacking = false;
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
        target.health -= 20;

        if (target.health <= 0)
        {
            target.isDying = true;
            target = null;
            attacking = false;
        }
    }

    private void FindTarget()
    {
        foreach (Fruit f in Game.Instance.fruits)
        {
            if (f.isDying)
                continue;

            Vector3 diff = transform.position - f.transform.position;
            //Debug.Log(diff.magnitude + " searching for target");

            if (diff.magnitude < 60)
            {
                //Debug.Log("target found");
                target = f;
                attacking = true;
                RotateTowardsTarget();
                break;
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
