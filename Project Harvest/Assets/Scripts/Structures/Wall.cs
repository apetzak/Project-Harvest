using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Structure
{
    public GameObject fence;
    public int stage = 0;
    public int dropTime = 60;
    public bool dropping;
    public float dropDistance;

    void Start()
    {
        
    }

    protected override void LeftClick()
    {
        Drop();

        //base.LeftClick();
    }

    public virtual void Drop()
    {
        stage++;
        fence.transform.Translate(0, -(dropDistance), 0, Space.World);
    }
}
