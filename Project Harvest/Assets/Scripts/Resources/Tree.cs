using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Resource
{
    private bool falling = false;
    private int fallCounter = 0;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (falling)
        {
            fallCounter++;
            transform.Rotate(.5f, 0, 0);

            if (fallCounter >= 90)
            {
                falling = false;
                Destroy(gameObject);
            }
        }
    }

    protected override void LeftClick()
    {
        falling = true;
    }

    private void FallOver()
    {

    }

    void OnMouseEnter()
    {
        if (Game.Instance.workerIsSelected)
            CursorSwitcher.Instance.Set(8);
    }
}
