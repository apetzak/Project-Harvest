using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatermelonPatch : Farm
{
    public Troop dummy;

    public override void Start()
    {
        growthEnd = 160;
        prop.transform.localScale = new Vector3(0, 0, 0);
        base.Start();
    }

    public override void GrowProp()
    {
        prop.transform.localScale += new Vector3(.01f, .01f, .01f);
    }

    public override void Update()
    {
        if (state == FarmState.Spawning)
        {

        }
        else
        {
            base.Update();
        }
    }

    public override void LeftClick()
    {
        if (state == FarmState.Spawning)
            return;

        if (state == FarmState.Empty)
        {
            dirtMesh.enabled = true;
            state = FarmState.Planting;
        }
        else if (state == FarmState.Planting)
        {
            propMesh.enabled = true;
            prop.transform.localScale = new Vector3(0, 0, 0);
            state = FarmState.Growing;
        }
        else if (state == FarmState.Pickable)
        {
            Troop t = Pick(1)[0];
            //t.transform.position = new Vector3(-.25f, 5.68f, -7.08f) + transform.position;
            //t.transform.Rotate(0, 0, -90, Space.Self);
            //t.transform.position = dummy.transform.position;
            //t.transform.rotation = dummy.transform.rotation;



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
    }
}
