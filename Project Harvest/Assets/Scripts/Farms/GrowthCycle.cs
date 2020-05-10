using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthCycle : MonoBehaviour
{
    public int tick;

    private void Start()
    {
        
    }

    private void Update()
    {
        tick++;
        if (tick < 60)
            return;
        tick = 0;

        foreach (Farm f in Game.Instance.fruitFarms)
        {

        }
    }
}
