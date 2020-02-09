using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub : Structure
{
    public GameObject prop;
    public List<GameObject> props;
    public int growthTimer;
    public int growthEnd;
    public int unitsGrown = 0;
    public int maxUnits;
    public bool spawning;
    public Worker workerPrefab;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnMouseEnter()
    {
        if (Game.Instance.troopIsSelected)
            CursorSwitcher.Instance.Set(1);

        else if (Game.Instance.workerIsSelected)
            CursorSwitcher.Instance.Set(3);
    }

    protected virtual void GrowUnits()
    {

    }

    protected virtual void ReleaseUnits()
    {

    }

    protected override void Update()
    {
        if (unitsGrown >= maxUnits) // stop growing units at max capacity
            return;

        growthTimer++;

        if (growthTimer >= growthEnd)
        {
            GrowUnits();
            growthTimer = 0;
        }
        base.Update();
    }

    protected override void LeftClick()
    {
        Pick();
        ReleaseUnits();
    }

    /// <summary>
    /// Spawn workers, hide props, restart growth cycle
    /// </summary>
    protected virtual void Pick()
    {
        for (int i = 0; i < unitsGrown; i++)
            SpawnWorker(i);

        ShowProps(false);
        unitsGrown = growthTimer = 0;
    }

    private void SpawnWorker(int i)
    {
        GameObject b = props[i];
        Worker w = Instantiate(workerPrefab, b.transform.position, b.transform.rotation);
        w.facingAngle = 90;
        var t = gameObject.transform.position;
        w.SetDestination(new Vector3(t.x + i * 5, t.y, t.z - 35));
        w.Spawn();
    }

    /// <summary>
    /// Enable or disable all prop objects.
    /// </summary>
    /// <param name="b"></param>
    protected void ShowProps(bool b = true)
    {
        foreach (GameObject prop in props)
            prop.GetComponent<MeshRenderer>().enabled = b;
    }
}
