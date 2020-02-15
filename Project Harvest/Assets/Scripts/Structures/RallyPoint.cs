using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyPoint : Structure
{
    public Material red;
    public Material green;

    protected override void Start()
    {
        var fabric = transform.GetChild(0).transform.GetChild(0);
        var mr = fabric.GetComponent<SkinnedMeshRenderer>();
        mr.material = fruit ? red : green;
        base.Start();
    }

    public override void Remove()
    {
        ClearPointOnFarms();
        base.Remove();
    }

    private void ClearPointOnFarms()
    {
        if (fruit)
        {
            foreach (Structure s in Game.Instance.fruitStructures)
            {
                if (s is Farm && (s as Farm).rallyPoint == transform.position)
                    (s as Farm).rallyPoint = new Vector3();
            }
        }
        else
        {
            foreach (Structure s in Game.Instance.veggieStructures)
            {
                if (s is Farm && (s as Farm).rallyPoint == transform.position)
                    (s as Farm).rallyPoint = new Vector3();
            }
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}
