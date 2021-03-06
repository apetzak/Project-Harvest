﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pear : Fruit
{
    public Burst burst;
    public Projectile rocket;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (rocket.hit) // explosion!
        {
            Burst b = Instantiate(burst, rocket.hitLocation, Quaternion.identity);
            b.Spawn();
            rocket.mesh.enabled = true;
            rocket.hit = false;
            rocket.target.health -= attackDamage;
        }
        base.Update();
    }

    public override void InflictDamage()
    {
        rocket.Spawn(destination, target);
        rocket.mesh.enabled = false;
    }
}
