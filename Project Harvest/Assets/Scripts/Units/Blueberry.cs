using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueberry : Worker
{
    void Start()
    {
        transform.Rotate(0, 90, 0, Space.Self);
        base.Start();
    }

    void Update()
    {
        base.Update();
    }
}
