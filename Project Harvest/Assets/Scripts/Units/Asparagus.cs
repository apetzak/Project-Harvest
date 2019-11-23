using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asparagus : Troop
{
    public Burst burst;

    void Start()
    {
        transform.Rotate(0, 90, 0, Space.Self);
        base.Start();
    }

    void Update()
    {
        base.Update();
    }

    public override void TriggerAttack()
    {
        //SoundFX.Instance.Play();
        burst.Pop();
        base.TriggerAttack();
    }
}