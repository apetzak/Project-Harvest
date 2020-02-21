using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Wall : Structure
{
    public GameObject fence;
    public int stage = 0;
    public int dropTime = 60;
    public bool dropping;
    public float dropDistance;
    private bool dropNext = true;

    public override void TakeDamage()
    {
        if (health < 100 && stage == 3)
            Drop();
        else if (health < 200 && stage == 2)
            Drop();
        else if (health < 300 && stage == 1)
            Drop();
        else if (health < 400 && stage == 0)
            Drop();

        base.TakeDamage();
    }

    public virtual void Drop()
    {
        stage++;
        fence.transform.Translate(0, -(dropDistance), 0, Space.World);
        dropNext = true;
    }
}
