using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : Fruit
{
    public Projectile arrow;

    void Start()
    {
        transform.Rotate(0, 0, -70, Space.Self);
        transform.Translate(0, 4f, 0, Space.World);
        base.Start();
    }

    void Update()
    {
        if (arrow.hit)
        {
            arrow.hit = false;
            arrow.target.health -= attackDamage;
        }
        base.Update();
    }

    public override void InflictDamage()
    {
        arrow.Spawn(destination, target);
    }

    public override void RotateTowards(float x, float z)
    {
        float diff = GetAngle(x, z);
        transform.Rotate(0, diff, 0, Space.World);
        facingAngle += diff;
    }
}
