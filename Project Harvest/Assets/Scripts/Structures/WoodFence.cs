using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodFence : Wall
{
    public override void Drop()
    {
        base.Drop();

        if (stage == 4)
            Destroy(gameObject);
    }
}
