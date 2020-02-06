using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Structure
{
    public GameObject gun;
    public Burst burst;
    private int attackTime = 60;

    void Start()
    {

    }

    void Update()
    {
        attackTime--;

        if (attackTime <= 0)
        {
            burst.Pop();
            attackTime = 60;
        }

        gun.transform.Rotate(0, 1f, 0, Space.World);
    }
}
