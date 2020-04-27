using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI : MonoBehaviour
{
    private class StructLoc
    {
        public Vector3 Pos;
        public string Structure;

        public StructLoc(int xLoc, int zLoc, string s)
        {
            Pos = new Vector3(xLoc, 0, zLoc);
            Structure = s;
        }
    }

    public int tickCount = 0;
    private int unitSpawnCount = 1;
    private BlueberryBush fruitHub;
    private PeaPlant veggieHub;
    private List<StructLoc> fruitLocations;
    private List<StructLoc> veggieLocations;

    void Start()
    {
        fruitHub = Game.Instance.fruitStructures[0] as BlueberryBush;
        veggieHub = Game.Instance.veggieStructures[0] as PeaPlant;
        fruitLocations = new List<StructLoc>();
        fruitLocations.Add(new StructLoc(0, 50, "AppleTree"));
        fruitLocations.Add(new StructLoc(50, 50, "PearTree"));
        fruitLocations.Add(new StructLoc(0, 80, "StrawberryBush"));
        fruitLocations.Add(new StructLoc(20, 80, "StrawberryBush"));
        fruitLocations.Add(new StructLoc(40, 80, "StrawberryBush"));
        fruitLocations.Add(new StructLoc(-50, 50, "WatermelonPatch"));
        fruitLocations.Add(new StructLoc(-50, 90, "BananaTree"));
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
        Vector3 pos = fruitHub.transform.position + new Vector3(xLoc, 0, zLoc);
        Structure s = Instantiate(Assets.GetStructure(type), pos, Quaternion.identity);
        if (s.OverlapsExistingStructure())
        {
            Destroy(s);
            return;
        }
        s.fruit = !Game.Instance.fruit;
        s.Place();
        Material m = s.fruit ? Assets.GetMaterial("Fruit") : Assets.GetMaterial("Veggie");
        s.ToggleSelectorColor(m);
        s.ToggleSelector();
    }

    private void PlaceFarm(StructLoc sl)
    {
        Vector3 pos = fruitHub.transform.position + sl.Pos;
        Structure s = Instantiate(Assets.GetStructure(sl.Structure), pos, Quaternion.identity);
        s.fruit = !Game.Instance.fruit;
    }

    private void PickResource(Worker w, int i)
    {
        if (i == 0)
            w.SwitchState(Worker.State.WoodCutting);
        else if (i == 1)
            w.SwitchState(Worker.State.GoldMining);
        else if (i == 2)
            w.SwitchState(Worker.State.StoneMining);

        w.FindNearestResource();
    }

    private void Update()
    {
        tickCount++;
        if (tickCount < 180)
            return;
        tickCount = 0;

        System.Random r = new System.Random();

        if (!Game.Instance.fruit)
        {
            if (fruitHub.unitsGrown > unitSpawnCount)
            {
                fruitHub.Pick();
                unitSpawnCount = r.Next(1, 8);
                return;
            }

            if (!HasStructure(typeof(LumberMill)) && Game.Instance.fruitResourceWood >= 100)
            {
                PlaceStructure("LumberMill", -50, -50);
                return;
            }

            if (!HasStructure(typeof(MiningCamp)) && Game.Instance.fruitResourceWood >= 100)
            {
                PlaceStructure("MiningCamp", -150, -50);
                return;
            }

            int index = -1;
            foreach (StructLoc sl in fruitLocations)
            {
                if (Assets.GetStructure(sl.Structure).CanAfford())
                {
                    PlaceFarm(sl);
                    index = fruitLocations.IndexOf(sl);
                    break;
                }
            }

            if (index != -1)
            {
                fruitLocations.RemoveAt(index);
                return;
            }

            //if (fruitLocations[0].structure.CanAfford())
            //{
            //    PlaceFarm(fruitLocations[0]);
            //    return;
            //}

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
                            w.TargetStructure(s);
                    }
                    return;
                }
            }

            // pick tree if mining camp doesn't exist
            int i = HasStructure(typeof(MiningCamp)) ? r.Next(0, 3) : 0;

            foreach (Worker w in Game.Instance.blueberries)
            {
                if (!w.moving && w.state == Worker.State.Idle)
                    PickResource(w, i);
            }
        }
        else
        {

        }
    }
}
