using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitUtils
{
    public static void ClearTargets()
    {
        foreach (Unit u in Game.Instance.selectedUnits)
        {
            u.target = null;

            if (u is Troop)
                (u as Troop).attacking = false;
        }
    }

    public static void ClearUnitSelection()
    {
        foreach (Unit u in Game.Instance.selectedUnits)
        {
            if (u != null && u.gameObject != null)
                u.ToggleSelected(false);
            else
            {
                Debug.Log("not null");
            }
        }
        Game.Instance.selectedUnits = new List<Unit>();
    }

    public static void SetGroupLocation(RaycastHit hit)
    {
        int count = Game.Instance.selectedUnits.Count;
        double rows = Math.Round(Math.Sqrt(count), 0);
        float xSpace = 0;
        float zSpace = 0;
        float lowestSpeed = 20;

        // create formation
        foreach (Unit u in Game.Instance.selectedUnits)
        {
            if (u.isDying) // todo: figure out why dead units are in here
            {              // might be fixed already
                Debug.Log("dead in selected");
                continue;
            }

            if (u.speed < lowestSpeed)
                lowestSpeed = u.speed;

            rows--;
            if (rows <= 1)
            {
                xSpace = 0;
                zSpace += 5;
                rows = Math.Round(Math.Sqrt(count), 0);
            }
            xSpace += 5;
            u.SetDestination(new Vector3(hit.point.x + xSpace, hit.point.y, hit.point.z + zSpace));
        }

        foreach (Unit u in Game.Instance.selectedUnits)
            u.currentSpeed = lowestSpeed;
    }
}