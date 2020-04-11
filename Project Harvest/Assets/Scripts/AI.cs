using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (!Game.Instance.fruit)
        {
            if ((Game.Instance.fruitStructures[0] as Hub).unitsGrown > 3)
                (Game.Instance.fruitStructures[0] as Hub).Pick();

            foreach (Worker w in Game.Instance.blueberries)
            {
                if (!w.moving && w.state == Worker.State.Idle)
                {

                }
            }

        }
        else
        {
            if (Game.Instance.veggieStructures.Count == 1)
            {

            }

            if ((Game.Instance.veggieStructures[0] as Hub).unitsGrown > 3)
                (Game.Instance.veggieStructures[0] as Hub).Pick();

            foreach (Worker w in Game.Instance.peas)
            {
                if (!w.moving && w.state == Worker.State.Idle)
                {

                }
            }
        }
    }


}
