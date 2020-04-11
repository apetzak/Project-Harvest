using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// Produces units or provides upgrades/utility/defense.
/// Can be built or repaired by workers (using stone and wood).
/// Can be destroyed by troops.
/// </summary>
public class Structure : Entity
{
    public GameObject ring;
    public float minX;
    public float maxX;
    public float maxZ;
    public float minZ;
    public float height;
    public bool isPlaced = false;
    public bool isBuilt;
    public bool isVisible;
    private float div = .1f;
    public List<Slot> slots = new List<Slot>();
    public int woodCost;
    public int stoneCost;
    public int goldCost;

    /// <summary>
    /// Set health to maxHealth, SetBounds(), CreateSelector()
    /// </summary>
    protected virtual void Start()
    {
        health = maxHealth;
        SetBounds();
        CreateSelector();
    }

    public void SetBounds()
    {
        var c = GetComponent<Collider>();
        minX = transform.position.x - c.bounds.extents.x;
        maxX = transform.position.x + c.bounds.extents.x;
        minZ = transform.position.z - c.bounds.extents.z;
        maxZ = transform.position.z + c.bounds.extents.z;
        height = c.bounds.extents.y * 2;
    }

    public void CreateSelector()
    {
        if (selector == null || selector.transform.childCount == 0)
            return;

        var sides = new List<Transform>();

        for (int i = 0; i < 4; i++)
        {
            sides.Add(selector.transform.GetChild(i));
            sides[i].localPosition = new Vector3();
            sides[i].GetComponent<MeshRenderer>().material =
                fruit ? Assets.GetMaterial("Fruit") : Assets.GetMaterial("Veggie");
        }

        var c = GetComponent<Collider>();

        var v1 = sides[0].transform.localScale;  
        sides[0].transform.localScale = sides[1].transform.localScale =
            new Vector3(c.bounds.extents.x * 2 + 2, v1.y, v1.z);

        var v2 = sides[2].transform.localScale;
        sides[2].transform.localScale = sides[3].transform.localScale =
            new Vector3(v2.x, v2.y, c.bounds.extents.z * 2 + 1);

        var xDist = c.bounds.extents.x + .5f;
        var zDist = c.bounds.extents.z + .5f;

        sides[0].Translate(0, 0, zDist, Space.Self);
        sides[1].Translate(0, 0, -zDist, Space.Self);
        sides[2].Translate(xDist, 0, 0, Space.Self);
        sides[3].Translate(-xDist, 0, 0, Space.Self);
    }

    public void CreateSlots()
    {
        float horizLength = selector.transform.GetChild(0).transform.localScale.x;
        float vertLength = selector.transform.GetChild(2).transform.localScale.z;

        while (horizLength > 0)
        {
            var loc = new Vector3(minX + horizLength, 10, maxZ + 3);
            var loc2 = new Vector3(minX + horizLength, 10, minZ - 3);

            slots.Add(new Slot(loc));
            slots.Add(new Slot(loc2));

            horizLength -= 5;
        }

        while (vertLength > 0)
        {
            var loc = new Vector3(maxX + 3, 10, minZ + vertLength);
            var loc2 = new Vector3(minX - 3, 10, minZ + vertLength);

            slots.Add(new Slot(loc));
            slots.Add(new Slot(loc2));

            vertLength -= 5;
        }
    }

    public void ClearSlots()
    {
        foreach (Slot s in slots)
            s.occupied = false;
    }

    public virtual void Init()
    {

    }

    /// <summary>
    /// Empty
    /// </summary>
    protected virtual void Update()
    {

    }

    /// <summary>
    /// base
    /// </summary>
    protected override void LeftClick()
    {
        if (isPlaced)
            base.LeftClick();
    }

    /// <summary>
    /// SendTroopsToAttack(), SendWorkersToBuild()
    /// </summary>
    protected override void RightClick()
    {
        SendTroopsToAttack();

        //SendWorkersToDeposit();
        SendWorkersToBuild();
    }

    protected virtual void SendTroopsToAttack()
    {
        if (Game.Instance.troopIsSelected && !IsAlly())
        {
            foreach (Unit t in Game.Instance.selectedUnits)
            {
                if (t is Troop)
                    (t as Troop).TargetStructure(this);
            }
        }
    }

    private void SendWorkersToDeposit()
    {
        //if (isBuilt && Game.Instance.workerIsSelected)
        //{
        //    foreach (Unit u in Game.Instance.selectedUnits)
        //    {
        //        if (u is Troop)
        //            continue;


        //        if (u is Worker && (u as Worker).resourceCount > 0)
        //        {
        //            Worker w = u as Worker;

        //            Type t = state == State.WoodCutting ? typeof(LumberMill) :
        //            state == State.CollectingWater ? typeof(WaterWell) : typeof(MiningCamp);

        //            u.SetDestination(GetWorkerDestination(u as Worker));
        //        }
        //    }
        //}
    }

    public void ConsumeBuildingCost()
    {
        if (fruit)
        {
            Game.Instance.fruitResourceWood -= woodCost;
            Game.Instance.fruitResourceStone -= stoneCost;
            Game.Instance.fruitResourceGold -= goldCost;
        }
        else
        {
            Game.Instance.veggieResourceWood -= woodCost;
            Game.Instance.veggieResourceStone -= stoneCost;
            Game.Instance.veggieResourceGold -= goldCost;
        }
    }

    public bool BoundsOverlap(Structure s)
    {
        if (s.maxX < minX ||
            s.minX > maxX ||
            s.maxZ < minZ ||
            s.minZ > maxZ)
            return false;
        return true;
    }

    public void Place()
    {
        ConsumeBuildingCost();

        if (fruit)
            Game.Instance.fruitStructures.Add(this);
        else
            Game.Instance.veggieStructures.Add(this);

        isPlaced = true;
        health = 1;

        if (!(this is Farm))
        {
            CreateSlots();
            transform.GetChild(0).transform.Translate(0, -height, 0, Space.World);
            SendWorkersToBuild();
        }
        else
        {
            (this as Farm).FindRallyPoint();
            (this as Farm).ShowGrass();
        }
    }

    public void SendWorkersToBuild()
    {
        //Debug.Log(Game.Instance.workerIsSelected && health < maxHealth);

        if (Game.Instance.workerIsSelected && health < maxHealth && !(this is Farm))
        {
            foreach (Unit u in Game.Instance.selectedUnits)
            {
                if (u.fruit == fruit && u is Worker)
                {
                    var slot = GetOpenSlot(u);
                    if (slot == new Vector3())
                        return;
                    u.target = this;
                    u.SetDestination(GetOpenSlot(u));
                    (u as Worker).SwitchState(Worker.State.Building);
                }
            }
        }
    }

    /// <summary>
    /// Show ring, switch cursor
    /// </summary>
    protected override void OnMouseEnter()
    {
        if (!isPlaced || Game.Instance.mouseOverUI)
            return;

        ToggleRing();
        ToggleSelector();

        base.OnMouseEnter();
    }

    /// <summary>
    /// Toggle off ring if not selected
    /// </summary>
    protected override void OnMouseExit()
    {
        if (!isPlaced)
            return;

        if (!selected)
        {
            ToggleRing(false);
            ToggleSelector(false);
        }
        base.OnMouseExit();
    }

    /// <summary>
    /// Show area ring
    /// </summary>
    /// <param name="b"></param>
    public void ToggleRing(bool b = true)
    {
        if (ring != null)
            ring.GetComponent<MeshRenderer>().enabled = b;
    }

    /// <summary>
    /// Show selector
    /// </summary>
    /// <param name="b"></param>
    public void ToggleSelector(bool b = true)
    {
        if (selector == null || selector.transform.childCount == 0)
            return;

        if (!isBuilt)
            selector.SetActive(true);
        else
            selector.SetActive(b);
    }

    /// <summary>
    /// Change selector material
    /// </summary>
    /// <param name="m"></param>
    public void ToggleSelectorColor(Material m)
    {
        if (selector != null && selector.transform.childCount > 0)
        {
            for (int i = 0; i < 4; i++)
                selector.transform.GetChild(i).GetComponent<MeshRenderer>().material = m;
        }
    }

    /// <summary>
    /// Play explosion, remove from collection, destroy
    /// </summary>
    public override void Remove()
    {
        Audio.Instance.PlayExplosion();

        if (fruit)
            Game.Instance.fruitStructures.Remove(this);
        else
            Game.Instance.veggieStructures.Remove(this);
       
        Destroy(gameObject);
    }

    /// <summary>
    /// Enable/disable selector and ring
    /// </summary>
    /// <param name="b">If true, add this to selectedStructures</param>
    public override void ToggleSelected(bool b)
    {
        if (b)
            Game.Instance.selectedStructures.Add(this);

        ToggleRing(b);
        ToggleSelector(b);
        selected = b;
    }

    public override void SelectType()
    {
        int selectedCount = 0;
        var list = fruit ? Game.Instance.fruitStructures : Game.Instance.veggieStructures;

        foreach (Structure s in list)
        {
            if (selectedCount > 84)
                break;

            if (s.fruit != fruit || s.GetType() != GetType())
                continue;

            selectedCount++;
            s.ToggleSelected(true);
            s.ToggleRing(true);
        }

        Game.Instance.ChangeSelection();
    }

    public void Build()
    {
        if (health == maxHealth)
        {
            ClearSlots();
            return;
        }

        if (maxHealth - health > 10)
        {
            health += 10;
        }
        else
        {
            ClearSlots();
            health = maxHealth;
            transform.GetChild(0).transform.Translate(0, height / 10, 0, Space.World);
            isBuilt = true;
            ToggleSelector(false);

            if (this is RallyPoint)
                (this as RallyPoint).SetPointOnFarms();
            else if (this is WaterTower)
                (this as WaterTower).ActivateSprinklers();
            else if (this is Sprinkler)
                (this as Sprinkler).SetSource();
        }

        // rise out of ground
        if (health >= maxHealth * div)
        {
            transform.GetChild(0).transform.Translate(0, height / 10, 0, Space.World);
            div += .1f;
        }
    }

    public Vector3 GetOpenSlot(Unit u)
    {
        Slot s = new Slot(new Vector3());
        float closest = 100000;

        foreach (Slot slot in slots)
        {
            if (slot.occupied == true)
                continue;
            float dist = (slot.location - u.transform.position).sqrMagnitude;
            if (dist < closest)
            {
                closest = dist;
                s = slot;
            }
        }

        s.occupied = true;
        return s.location;
    }

    public bool HasOpenSpot()
    {
        foreach (Slot s in slots)
        {
            if (!s.occupied)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Gets the location of closest edge relative to the unit position
    /// </summary>
    /// <param name="u"></param>
    /// <returns></returns>
    public Vector3 GetUnitDestination(Unit u)
    {
        Vector3 v = transform.position;

        float x = transform.position.x;
        float z = transform.position.z;

        float dist = 1f;

        if (GetType() == typeof(Tree))
            dist = -2f;
        else if (this is Farm)
            dist = -4f;

        if (u.transform.position.x < minX)
            x = minX - dist;
        else if (u.transform.position.x > maxX)
            x = maxX + dist;

        else if (u.transform.position.z < minZ)
            z = minZ - dist;
        else if (u.transform.position.z > maxZ)
            z = maxZ + dist;

        return new Vector3(x, 0, z);
    }

    private void OnBecameVisible()
    {
        //Debug.Log("visible " + gameObject);
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }
}

public class Slot
{
    public Vector3 location;
    public bool occupied;

    public Slot (Vector3 loc)
    {
        location = loc;
        occupied = false;
    }
}