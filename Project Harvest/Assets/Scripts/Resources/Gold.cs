using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Resource
{
    protected override void Start()
    {
        workerstate = Worker.State.GoldMining;
        cursorIndex = 6;

        // pick one of four props
        int var = Random.Range(0, 3);
        prop = transform.GetChild(var).gameObject;

        DeleteOtherProps();
        base.Start();
    }

    private void DeleteOtherProps()
    {
        for (int i = 3; i >= 0; i--)
        {
            if (transform.GetChild(i).gameObject != prop)
                Destroy(transform.GetChild(i).gameObject);
        }
    }

    protected override void LeftClick()
    {
        Shrink();
    }
}
