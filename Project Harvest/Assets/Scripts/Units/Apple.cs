﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Fruit
{
    public GameObject sword;
    public Vector3 initPosition;
    public int swingTimer = 20;
    public bool swinging = false;

    protected override void Start()
    {
        initPosition = sword.transform.position;
        base.Start();
    }

    protected override void Update()
    {
        if (swinging)
            SwingSword();

        base.Update();
    }

    public override void InflictDamage()
    {
        swinging = true;
    }

    private void SwingSword()
    {
        //StopMoving();

        if (swingTimer > 9)
        {
            sword.gameObject.transform.Rotate(0, 12, 0, Space.Self);
        }
        else if (swingTimer >= 0)
        {
            sword.gameObject.transform.Rotate(0, -12, 0, Space.Self);
        }
        else
        {
            swinging = false;
            swingTimer = 20;
            base.InflictDamage();
            //sword.gameObject.transform.Rotate(0, -120, 0, Space.Self);
        }

        swingTimer--;
    }
}
