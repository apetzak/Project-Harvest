using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI : MonoBehaviour
{
    public int tickCount = 0;

    void Start()
    {
        
    }

    private bool HasStructure(Type t)
    {
        foreach (Structure s in Game.Instance.fruitStructures)
        {
            if (s.GetType() == t)
                return true;
        }
        return false;
    }

    private void PlaceStructure(string type, int xLoc, int zLoc)
    {
        Vector3 pos = Game.Instance.fruitStructures[0].transform.position + new Vector3(xLoc, 0, zLoc);
        Structure s = Instantiate(Assets.GetStructure(type), pos, Quaternion.identity);
        s.fruit = !Game.Instance.fruit;
        Material m = s.fruit ? Assets.GetMaterial("Fruit") : Assets.GetMaterial("Veggie");
        s.ToggleSelectorColor(m);
    }

    private void PlaceFarm(int i)
    {
        Type t = i == 0 ? typeof(AppleTree) : i == 1 ? typeof(BananaTree) : i == 2 ? typeof(PearTree) : i == 3 ? typeof(StrawberryBush) : typeof(WatermelonPatch);

        Vector3 pos = Game.Instance.fruitStructures[0].transform.position + new Vector3(0, 0, 0);
        Structure s = Instantiate(Assets.GetStructure(t.Name), pos, Quaternion.identity);
        s.fruit = !Game.Instance.fruit;
        //Material m = s.fruit ? Assets.GetMaterial("Fruit") : Assets.GetMaterial("Veggie");
        //s.ToggleSelectorColor(m);
    }

    void Update()
    {
        tickCount++;
        if (tickCount < 180)
            return;

        if (!Game.Instance.fruit)
        {
            if ((Game.Instance.fruitStructures[0] as Hub).unitsGrown > 3)
                (Game.Instance.fruitStructures[0] as Hub).Pick();

            if (!HasStructure(typeof(LumberMill)) && Game.Instance.fruitResourceWood >= 100)
            {
                PlaceStructure("LumberMill", -50, -50);
                return;
            }

            if (!HasStructure(typeof(MiningCamp)) && Game.Instance.fruitResourceWood >= 100)
            {
                PlaceStructure("MiningCamp", 0, 75);
                return;
            }

            System.Random r = new System.Random();

            //if (Game.Instance.fruitResourceGold > 250)
            //{
            //    PlaceFarm(r.Next(0, 5));
            //    return;
            //}
            
            foreach (Structure s in Game.Instance.fruitStructures)
            {
                if (!s.isBuilt)
                {
                    foreach (Worker w in Game.Instance.blueberries)
                    {
                        if (!w.moving && w.state == Worker.State.Idle)
                        {
                            var slot = s.GetOpenSlotLocation(w);
                            if (slot == new Vector3())
                                return;
                            w.target = s;
                            w.SetDestination(slot);
                            w.SwitchState(Worker.State.Building);
                        }
                    }
                    return;
                }
            }

            foreach (Worker w in Game.Instance.blueberries)
            {
                if (!w.moving && w.state == Worker.State.Idle)
                {
                    int i = r.Next(0, 3);

                    if (i == 0)
                        w.SwitchState(Worker.State.WoodCutting);
                    else if (i == 1)
                        w.SwitchState(Worker.State.GoldMining);
                    else if (i == 2)
                        w.SwitchState(Worker.State.StoneMining);
                    //Debug.Log(i);
                    w.FindNearestResource();
                }
            }
        }
        else
        {

        }

        tickCount = 0;
    }
}
