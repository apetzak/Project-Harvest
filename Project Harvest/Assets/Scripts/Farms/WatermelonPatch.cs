using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatermelonPatch : Farm
{
    public Troop dummy;

    protected override void Start()
    {
        growthEnd = 160;
        prop.transform.localScale = new Vector3(0, 0, 0);
        index = 8;
        base.Start();
    }

    protected override void GrowProp()
    {
        prop.transform.localScale += new Vector3(.01f, .01f, .01f);
    }

    protected override void Update()
    {
        if (state == State.Spawning)
        {
            // todo
        }
        else
        {
            base.Update();
        }
    }

    protected override void LeftClick()
    {
        if (state == State.Spawning)
            return;

        if (state == State.Empty)
        {
            StartPlanting();
        }
        else if (state == State.Planting)
        {
            prop.transform.localScale = new Vector3(0, 0, 0);
            StartGrowing();
        }
        else if (state == State.Pickable)
        {
            Pick(1);
            spawnTime = spawnStart;
            MoveToRallyPoint();
            //t.transform.position = new Vector3(-.25f, 5.68f, -7.08f) + transform.position;
            //t.transform.Rotate(0, 0, -90, Space.Self);
            //t.transform.position = dummy.transform.position;
            //t.transform.rotation = dummy.transform.rotation

            //if (propMesh != null)
            //    propMesh.enabled = false;
            //dirtMesh.enabled = false;
            //Debug.Log("pick");
            //Troop t = Instantiate(prefab, dummy.transform.position, dummy.transform.rotation);
            //t.transform.Rotate(0, -180, -90, Space.World);
            //t.transform.Translate(0, -8, 0, Space.World);
            //Game.Instance.troops.Add(t);
            //t.ToggleSelected(true);
            //state = FarmState.Empty;
        }

        base.LeftClick();
    }
}
