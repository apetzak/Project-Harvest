using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : Veggie
{
    private int spinTimer = 30;
    private bool spinning = false;
    public GameObject katana;

    void Start()
    {
        transform.Rotate(90, 0, 180, Space.Self);
        transform.Translate(0, 5, 0, Space.World);
        base.Start();
    }

    void Update()
    {
        if (spinning && !isDying)
            Spin();
        base.Update();
    }

    void Spin()
    {
        if (spinTimer >= 0)
        {
            gameObject.transform.Rotate(0, 0, -12, Space.Self);
        }
        else
        {
            spinning = false;
            spinTimer = 30;
            katana.gameObject.transform.Rotate(0, -90, 0, Space.Self);
            base.TriggerAttack();
        }

        spinTimer--;
    }

    public override void TriggerAttack()
    {
        spinning = true;
        katana.gameObject.transform.Rotate(0, 90, 0, Space.Self);
    }

    public override void RotateTowards(float x, float z)
    {
        if (spinning)
            return;

        float diff = GetAngle(x, z);
        transform.Rotate(0, 0, -diff, Space.Self);
        facingAngle += diff;
    }
}
