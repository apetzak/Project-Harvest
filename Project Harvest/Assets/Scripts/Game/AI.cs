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

    private void Start()
    {
        fruitHub = Game.Instance.fruitStructures[0] as BlueberryBush;
        veggieHub = Game.Instance.veggieStructures[0] as PeaPlant;
        fruitLocations = new List<StructLoc>();

        fruitLocations.Add(new StructLoc(-50, -50, "LumberMill"));
        fruitLocations.Add(new StructLoc(-150, -50, "MiningCamp"));
        fruitLocations.Add(new StructLoc(-50, -125, "Windmill"));
        fruitLocations.Add(new StructLoc(50, -125, "Windmill"));
        fruitLocations.Add(new StructLoc(50, 70, "Sprinkler"));
        fruitLocations.Add(new StructLoc(-50, 70, "Sprinkler"));
        fruitLocations.Add(new StructLoc(50, 170, "Sprinkler"));
        fruitLocations.Add(new StructLoc(-50, 170, "Sprinkler"));
        fruitLocations.Add(new StructLoc(-20, 120, "WaterTower"));
        fruitLocations.Add(new StructLoc(15, 120, "RallyPoint"));
        fruitLocations.Add(new StructLoc(-60, 0, "CompostBin"));
        fruitLocations.Add(new StructLoc(-80, 0, "CompostBin"));
        fruitLocations.Add(new StructLoc(-100, 0, "CompostBin"));
        fruitLocations.Add(new StructLoc(75, 0, "Turret"));
        fruitLocations.Add(new StructLoc(75, 225, "Turret"));
        AddFruitFarms();
    }

    private void AddFruitFarms()
    {
        fruitLocations.Add(new StructLoc(-85, 150, "AppleTree"));
        fruitLocations.Add(new StructLoc(-85, 90, "AppleTree"));
        fruitLocations.Add(new StructLoc(85, 150, "PearTree"));
        fruitLocations.Add(new StructLoc(85, 90, "PearTree"));
        fruitLocations.Add(new StructLoc(-35, 80, "StrawberryBush"));
        fruitLocations.Add(new StructLoc(-20, 80, "StrawberryBush"));
        fruitLocations.Add(new StructLoc(-5, 80, "StrawberryBush"));
        fruitLocations.Add(new StructLoc(-35, 60, "StrawberryBush"));
        fruitLocations.Add(new StructLoc(-20, 60, "StrawberryBush"));
        fruitLocations.Add(new StructLoc(-5, 60, "StrawberryBush"));
        fruitLocations.Add(new StructLoc(-35, 40, "StrawberryBush"));
        fruitLocations.Add(new StructLoc(-20, 40, "StrawberryBush"));
        fruitLocations.Add(new StructLoc(-5, 40, "StrawberryBush"));
        fruitLocations.Add(new StructLoc(-25, 200, "WatermelonPatch"));
        fruitLocations.Add(new StructLoc(25, 200, "WatermelonPatch"));
        fruitLocations.Add(new StructLoc(30, 80, "BananaTree"));
        fruitLocations.Add(new StructLoc(30, 50, "BananaTree"));
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
            if (Game.Instance.blueberries.Count < 50 && fruitHub.unitsGrown > unitSpawnCount)
            {
                fruitHub.Pick();
                unitSpawnCount = r.Next(1, 8);
            }

            if (HasFarm())
            {
                foreach (Worker w in Game.Instance.blueberries)
                {
                    if (!w.moving && w.state == Worker.State.Idle && UnityEngine.Random.value > .8f)
                        w.InteractWithNearestFarm();
                }
            }

            foreach (Worker w in Game.Instance.blueberries)
            {
                if (!w.moving && w.state == Worker.State.Idle && UnityEngine.Random.value > .5f)
                {
                    w.FindNearestBuilding();

                    if (w.state == Worker.State.Idle)
                        w.FindNearestBuilding();
                }
            }

            if (fruitLocations.Count > 0)
            {
                StructLoc sl = fruitLocations[0];
                Structure structure = Assets.GetStructure(sl.Structure);
                structure.fruit = !Game.Instance.fruit;
                if (structure.CanAfford())
                {
                    PlaceStructure(structure, sl);
                    fruitLocations.RemoveAt(0);
                    return;
                }
            }

            //if (Game.Instance.fruits.Count > 90 && !Game.Instance.fruits[0].attacking)
            //{
            //    foreach (Troop t in Game.Instance.fruits)
            //        t.FindClosestTarget();
            //    return;
            //}

            // pick tree if mining camp doesn't exists
            int i = GetResourceToPick();
            foreach (Worker w in Game.Instance.blueberries)
            {
                if (!w.moving && w.state == Worker.State.Idle && UnityEngine.Random.value > .5f)
                {
                    PickResource(w, i);
                    if (w.diff.sqrMagnitude > 10000 && UnityEngine.Random.value > .5f)
                        w.SwitchState(Worker.State.Idle);
                }
            }
        }
        else
        {

        }
    }

    private bool HasStructure(Type t)
    {
        foreach (Structure s in Game.Instance.fruitStructures)
        {
            if (s.GetType() == t && s.isBuilt)
                return true;
        }
        return false;
    }

    private bool HasFarm()
    {
        foreach (Structure s in Game.Instance.fruitFarms)
        {
            if (s is Farm)
                return true;
        }
        return false;
    }

    private int GetResourceToPick()
    {
        int i = 0;
        if (HasStructure(typeof(MiningCamp)))
        {
            if (Game.Instance.fruitResourceGold < Game.Instance.fruitResourceWood)
                i = 1;
            if (Game.Instance.fruitResourceStone < Game.Instance.fruitResourceGold)
                i = 2;
        }
        return i;
    }

    private void PlaceStructure(Structure prefab, StructLoc sl)
    {
        Vector3 pos = (Game.Instance.fruit ? veggieHub.transform.position : fruitHub.transform.position) + sl.Pos;
        Structure s = Instantiate(prefab, pos, Quaternion.identity);
        //s.SetBounds();
        if (s.OverlapsExistingStructure())
        {
            Debug.Log("overlap");
            //s.Remove();
            Destroy(s);
            return;
        }
        s.Place();
        Material m = s.fruit ? Assets.GetMaterial("Fruit") : Assets.GetMaterial("Veggie");
        s.ToggleSelectorColor(m);
        s.ToggleSelector();
        //Debug.Log("placed");
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
}
