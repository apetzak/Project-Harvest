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
    /// <summary>
    /// If the structure has been placed (created) by the player
    /// </summary>
    public bool isPlaced = false;
    /// <summary>
    /// If the workers have finished building the structure
    /// </summary>
    public bool isBuilt;
    public bool isVisible;
    private float growthMeter = .1f;
    public List<Slot> slots = new List<Slot>();
    public int woodCost;
    public int stoneCost;
    public int goldCost;

    /// <summary>
    /// Set health to maxHealth, SetBounds(), CreateSelector()
    /// </summary>
    protected virtual void Start()
    {
        health = 1;
        SetBounds();
        CreateSelector();
        name = GetType().Name;

        if (fruit != Game.Instance.fruit && !isBuilt)
            Place(); // created by AI
    }

    /// <summary>
    /// Calculate/set borders and height based on collider size
    /// </summary>
    public void SetBounds()
    {
        var c = GetComponent<Collider>();
        minX = transform.position.x - c.bounds.extents.x;
        maxX = transform.position.x + c.bounds.extents.x;
        minZ = transform.position.z - c.bounds.extents.z;
        maxZ = transform.position.z + c.bounds.extents.z;
        height = c.bounds.extents.y * 2;
    }

    /// <summary>
    /// Dynamically scale size of selector to match collider size
    /// </summary>
    public void CreateSelector()
    {
        if (selector == null || selector.transform.childCount == 0)
            return;

        var sides = new List<Transform>();

        for (int i = 0; i < 4; i++) // set color on each side
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

    /// <summary>
    /// Dynamically create unit slots based on size of the structure
    /// </summary>
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
    /// EMPTY
    /// </summary>
    protected virtual void Update()
    {

    }

    /// <summary>
    /// base if isPlaced
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
        if (!SendTroopsToAttack())
        {
            SendWorkersToDeposit();
            SendWorkersToBuild();
        }
    }

    protected virtual bool SendTroopsToAttack()
    {
        if (Game.Instance.troopIsSelected && !IsAlly())
        {
            foreach (Unit t in Game.Instance.selectedUnits)
            {
                if (t is Troop)
                    (t as Troop).TargetStructure(this);
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// todo: finish
    /// </summary>
    private void SendWorkersToDeposit()
    {
        if (!isBuilt || !Game.Instance.workerIsSelected)
            return;

        foreach (Unit u in Game.Instance.selectedUnits)
        {
            if (u is Troop)
                continue;

            if ((u as Worker).resourceCount > 0)
            {
                Worker w = u as Worker;

                if (this is LumberMill && w.state == Worker.State.WoodCutting)
                {
                    w.tools[9].SetActive(true); // sacking
                    u.SetDestination(GetUnitDestination(u));
                }
                else if (this is MiningCamp && (w.state == Worker.State.GoldMining || w.state == Worker.State.StoneMining))
                {
                    w.tools[9].SetActive(true); // sacking
                    u.SetDestination(GetUnitDestination(u));
                }
                else if (this is WaterWell && w.state == Worker.State.GatheringWater)
                {
                    w.tools[9].SetActive(true); // sacking
                    u.SetDestination(GetUnitDestination(u));
                }
                else if (this is CompostBin && w.state == Worker.State.GatheringWater)
                {
                    // todo
                    w.tools[9].SetActive(true); // sacking
                    u.SetDestination(GetUnitDestination(u));
                }
                else
                {
                    return;
                }
            }
        }
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

    public bool OverlapsExistingStructure()
    {
        foreach (Structure s in Game.Instance.fruitStructures)
        {
            if (BoundsOverlap(s))
                return true;
        }
        foreach (Structure s in Game.Instance.veggieStructures)
        {
            if (BoundsOverlap(s))
                return true;
        }
        foreach (Farm f in Game.Instance.fruitFarms)
        {
            if (BoundsOverlap(f))
                return true;
        }
        foreach (Farm f in Game.Instance.veggieFarms)
        {
            if (BoundsOverlap(f))
                return true;
        }
        foreach (Resource r in Game.Instance.resources)
        {
            if (BoundsOverlap(r))
                return true;
        }
        return false;
    }

    public virtual void Place()
    {
        ConsumeBuildingCost();

        if (fruit)
            Game.Instance.fruitStructures.Add(this);
        else
            Game.Instance.veggieStructures.Add(this);

        isPlaced = true;
        health = 1;

        CreateSlots();
        transform.GetChild(0).transform.Translate(0, -height, 0, Space.World); // sink into ground
        SendWorkersToBuild();
    }

    /// <summary>
    /// Command all selected workers to target this structure
    /// </summary>
    public void SendWorkersToBuild()
    {
        if (Game.Instance.workerIsSelected && health < maxHealth)
        {
            foreach (Unit u in Game.Instance.selectedUnits)
            {
                if (u.fruit == fruit && u is Worker)
                    (u as Worker).TargetStructure(this);
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
    public void ToggleRing(bool b = true)
    {
        if (ring != null)
            ring.GetComponent<MeshRenderer>().enabled = b;
    }

    /// <summary>
    /// Show selector
    /// </summary>
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
        AudioPlayer.Instance.PlayExplosion();

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

    /// <summary>
    /// Select all ally structures of the same type on double click.
    /// </summary>
    public override void SelectType()
    {
        int selectedCount = 0;
        var list = fruit ? Game.Instance.fruitStructures : Game.Instance.veggieStructures;

        foreach (Structure s in list)
        {
            if (selectedCount > 84)
                break;

            if (s.GetType() != GetType())
                continue;

            selectedCount++;
            s.ToggleSelected(true);
            s.ToggleRing(true);
        }

        Game.Instance.ChangeSelection();
    }

    /// <summary>
    /// Increment health and rise building, clear slots if finished
    /// </summary>
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
            OnBuilt();
        }

        // rise out of ground
        if (health >= maxHealth * growthMeter)
        {
            transform.GetChild(0).transform.Translate(0, height / 10, 0, Space.World);
            growthMeter += .1f;
        }
    }

    protected virtual void OnBuilt()
    {

    }

    /// <summary>
    /// Gets location of open slot nearest to the unit. Returns new Vector3() if no slot is open.
    /// </summary>
    public void SetOpenSlot(Unit u)
    {
        float closest = 100000;
        int slotIndex = -1;

        foreach (Slot slot in slots)
        {
            if (slot.occupied)
                continue;
            float dist = (slot.location - u.transform.position).sqrMagnitude;
            if (dist < closest)
            {
                closest = dist;
                slotIndex = slots.IndexOf(slot);
            }
        }

        if (slotIndex == -1)
        {
            u.slot = null;
            return;
        }

        slots[slotIndex].occupied = true;
        u.slot = slots[slotIndex];
        u.target = this;
        u.SetDestination(u.slot.location);
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

    public bool CanAfford()
    {
        //return true;
        if (fruit)
            return Game.Instance.fruitResourceWood >= woodCost
                && Game.Instance.fruitResourceStone >= stoneCost
                && Game.Instance.fruitResourceGold >= goldCost;
        return Game.Instance.veggieResourceWood >= woodCost
            && Game.Instance.veggieResourceStone >= stoneCost
            && Game.Instance.veggieResourceGold >= goldCost;
    }

    /// <summary>
    /// Gets the location of edge nearest to the unit position
    /// </summary>
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