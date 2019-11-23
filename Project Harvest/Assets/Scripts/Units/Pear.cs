using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pear : Troop
{
    public Burst burst;
    public Projectile rocket;

    void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (rocket.hit)
        {
            Burst b = Instantiate(burst, rocket.hitLocation, Quaternion.identity);
            b.Spawn();
            rocket.mesh.enabled = true;
            rocket.hit = false;
            base.TriggerAttack();
        }
        base.Update();
    }

    public override void TriggerAttack()
    {
        rocket.Spawn(destination);
        rocket.mesh.enabled = false;
    }
}
