using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watermelon : Troop
{
    public GameObject chainSaw;
    public Vector3 initPosition;
    private int swingTimer = 40;
    private bool swinging = false;

    void Start()
    {
        transform.Rotate(0, 180, 90, Space.World);
        transform.Translate(0, 8, 0, Space.World);
        base.Start();
    }

    void Update()
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
            base.TriggerAttack();
        }

        swingTimer--;
    }

    public override void TriggerAttack()
    {
        chainSaw.gameObject.transform.Rotate(65, 0, 0, Space.Self);
        swinging = true;
    }

    public override void RotateTowards(float x, float z)
    {
        if (swinging)
            return;
        float diff = GetAngle(x, z);
        transform.Rotate(diff, 0, 0, Space.Self);
        facingAngle += diff;
    }
}
