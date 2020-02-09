using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : Veggie
{
    private int spinTimer = 30;
    private bool spinning = false;
    public GameObject katana;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
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
            base.InflictDamage();
        }

        spinTimer--;
    }

    public override void InflictDamage()
    {
        spinning = true;
        katana.gameObject.transform.Rotate(0, 90, 0, Space.Self);
    }
}
