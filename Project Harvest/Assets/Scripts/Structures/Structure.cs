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

    /// <summary>
    /// Set health to maxHealth
    /// </summary>
    protected virtual void Start()
    {
        health = maxHealth;

        //Debug.Log($"{name} bounds: {c.bounds} | center: {transform.position}");
        //if (isPlaced)
            CreateSelector();
        SetBounds();
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

        for (int i = 0; i < 4; i++)
        {
            selector.transform.GetChild(i).localPosition = new Vector3();
            selector.transform.GetChild(i).GetComponent<MeshRenderer>().material =
                fruit ? TroopClass.Instance.materials[0] : TroopClass.Instance.materials[1];
        }

        var c = GetComponent<Collider>();

        var v1 = selector.transform.GetChild(0).transform.localScale;  
        selector.transform.GetChild(0).transform.localScale = 
        selector.transform.GetChild(1).transform.localScale =
            new Vector3(c.bounds.extents.x * 2 + 2, v1.y, v1.z);

        var v2 = selector.transform.GetChild(2).transform.localScale;
        selector.transform.GetChild(2).transform.localScale =
        selector.transform.GetChild(3).transform.localScale =
            new Vector3(v2.x, v2.y, c.bounds.extents.z * 2 + 1);

        var xDist = c.bounds.extents.x + .5f;
        var zDist = c.bounds.extents.z + .5f;

        selector.transform.GetChild(0).Translate(0, 0, zDist, Space.Self);
        selector.transform.GetChild(1).Translate(0, 0, -zDist, Space.Self);
        selector.transform.GetChild(2).Translate(xDist, 0, 0, Space.Self);
        selector.transform.GetChild(3).Translate(-xDist, 0, 0, Space.Self);
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
    /// TargetStructure() for all selected troops
    /// </summary>
    protected override void RightClick()
    {
        if (Game.Instance.troopIsSelected)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (!Physics.Raycast(ray, out hit))
                return;

            foreach (Unit t in Game.Instance.selectedUnits)
            {
                if (t is Troop)
                    (t as Troop).TargetStructure(this, new Vector3(hit.point.x, t.transform.position.y, hit.point.z));
            }
        }

        SendWorkersToDeposit();

        SendWorkersToBuild();
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

    public void SendWorkersToBuild()
    {
        //Debug.Log(Game.Instance.workerIsSelected && health < maxHealth);

        if (Game.Instance.workerIsSelected && health < maxHealth)
        {
            foreach (Unit u in Game.Instance.selectedUnits)
            {
                if (u.fruit == fruit && u is Worker)
                {
                    u.target = this;
                    u.SetDestination(GetWorkerDestination(u as Worker));
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
            return;

        if (maxHealth - health > 10)
        {
            health += 10;
        }
        else
        {
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

    public Vector3 GetWorkerDestination(Worker w)
    {
        Vector3 v = transform.position;

        float x = transform.position.x;
        float z = transform.position.z;

        float dist = 1f;

        if (GetType() == typeof(Tree))
            dist = -2f;

        if (w.transform.position.x < minX)
            x = minX - dist;
        else if (w.transform.position.x > maxX)
            x = maxX + dist;

        else if (w.transform.position.z < minZ)
            z = minZ - dist;
        else if (w.transform.position.z > maxZ)
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
