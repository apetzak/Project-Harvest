using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watermelon : Fruit
{
    public GameObject chainSaw;
    public Vector3 initPosition;
    private int swingTimer = 40;
    private bool swinging = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (swinging)
            Swing();
        base.Update();
    }

    void Swing()
    {
        if (swingTimer > 19)
        {
            chainSaw.gameObject.transform.Rotate(0, 3, 0, Space.Self);
        }
        else if (swingTimer >= 0)
        {
            chainSaw.gameObject.transform.Rotate(0, -3, 0, Space.Self);
        }
        else
        {
            swinging = false;
            swingTimer = 40;
            chainSaw.gameObject.transform.Rotate(-65, 0, 0, Space.Self);
            base.InflictDamage();
        }

        swingTimer--;
    }

    public override void InflictDamage()
    {
        chainSaw.gameObject.transform.Rotate(65, 0, 0, Space.Self);
        swinging = true;
    }
}
