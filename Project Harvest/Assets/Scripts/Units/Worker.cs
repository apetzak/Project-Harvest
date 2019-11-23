using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit
{
    public enum State
    {
        Idle,
        Raking,
        Watering,
        Mining,
        Chopping,
        Building,
        Planting
    }

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
