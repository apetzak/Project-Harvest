using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompostBin : Structure
{
    public GameObject lidClosed;
    public GameObject lidOpen;
    public int load = 0;
    public int loadCapacity = 10;
    public int fertilizeTime = 30;
    private int secondCounter = 60;
    public bool isFull = false;

    protected override void Start()
    {
        lidClosed.SetActive(false);
        lidOpen.SetActive(true);
        base.Start();
    }

    protected override void RightClick()
    {
        if (Game.Instance.workerIsSelected && !isFull && IsAlly() && isBuilt)
        {
            foreach (Unit u in Game.Instance.selectedUnits)
            {
                if (u.fruit == fruit && u is Worker && (u as Worker).state == Worker.State.Waggoning)
                {
                    u.target = this;
                    u.SetDestination(GetWorkerDestination(u as Worker));
                }
            }

            OpenLid();
        }

        base.RightClick();
    }

    protected override void Update()
    {
        if (!isBuilt || !isFull)
            return;

        secondCounter--;

        if (secondCounter <= 0)
        {
            fertilizeTime--;

            if (fertilizeTime == 0)
            {
                if (fruit)
                    Game.Instance.fruitResourceFertilizer += 100;
                else
                    Game.Instance.veggieResourceFertilizer += 100;

                OpenLid();
                load = 0;
                fertilizeTime = 30;
            }

            secondCounter = 60;
        }

        base.Update();
    }

    private void OpenLid()
    {
        isFull = !isFull;
        lidClosed.SetActive(isFull);
        lidOpen.SetActive(!isFull);
    }
}
