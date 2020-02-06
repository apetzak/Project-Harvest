using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneWall : Wall
{
    public GameObject column1;
    public GameObject column2;

    public override void Drop()
    {
        base.Drop();

        if (stage == 5)
            Destroy(gameObject);
    }
}
