using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : Structure
{
    public GameObject vane;
    public bool turnedOn = true;
    public int waterCollected = 0;
    private int counter = 60;

    protected override void Start()
    {
        // todo: check for others
        base.Start();
    }

    protected override void Update()
    {
        if (!isBuilt || !turnedOn)
            return;

        counter--;

        if (counter <= 0)
        {
            if (fruit)
                Game.Instance.fruitResourceWater += 1;
            else
                Game.Instance.veggieResourceWater += 1;
            waterCollected++;
            counter = 60;
        }

        vane.transform.Rotate(0, 0, 1);
        base.Update();
    }

    private void Activate()
    {

    }
}
