using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub : Structure
{
    public GameObject prop;
    public List<GameObject> props;
    public Worker workerPrefab;
    public int growthTimer;
    public int growthEnd;
    public int unitsGrown = 0;
    public int maxUnits;
    public bool spawning;

    void Start()
    {

    }

    protected override void OnMouseEnter()
    {
        CursorSwitcher.Instance.Set(3);
    }

    protected virtual void GrowUnits()
    {

    }

    protected virtual void ReleaseUnits()
    {

    }

    void Update()
    {
        if (unitsGrown >= maxUnits) // stop growing units at max capacity
            return;

        growthTimer++;

        if (growthTimer >= growthEnd)
        {
            GrowUnits(); // 
            growthTimer = 0;
        }
    }

    protected override void LeftClick()
    {
        Pick();
        ReleaseUnits();
    }

    protected virtual void Pick()
    {
        for (int i = 0; i < unitsGrown; i++)
            SpawnWorker(i);

        ShowProps(false);
        unitsGrown = growthTimer = 0;
    }

    void SpawnWorker(int i)
    {
        GameObject b = props[i];
        Worker w = Instantiate(workerPrefab, b.transform.position, b.transform.rotation);
        var t = gameObject.transform.position;
        w.SetDestination(new Vector3(t.x + i * 5, t.y, t.z - 35));
        w.Spawn();
    }

    protected void ShowProps(bool b = true)
    {
        foreach (GameObject prop in props)
            prop.GetComponent<MeshRenderer>().enabled = b;
    }
}
