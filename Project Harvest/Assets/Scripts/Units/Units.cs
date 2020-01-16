using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Units
{
    public static void ClearTargets()
    {
        if (Game.Instance.selectionCount == 0)
            return;

        foreach (Troop t in Game.Instance.troops)
        {
            if (t.target != null && t.selected)
            {
                t.target = null;
                t.attacking = false;
            }
        }
    }

    public static void DisableAllSelectors()
    {
        foreach (Unit u in Game.Instance.troops)
            u.ToggleSelected(false);
        foreach (Unit u in Game.Instance.workers)
            u.ToggleSelected(false);
    }

    public static void SetGroupLocation(RaycastHit hit)
    {
        var selected = Game.Instance.troops.FindAll(s => s.selected);
        int count = selected.Count;
        double rows = Math.Round(Math.Sqrt(count), 0);
        float xSpace = 0;
        float zSpace = 0;
        float lowestSpeed = 20;

        foreach (Troop u in selected)
        {
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

        foreach (Troop u in selected)
            u.currentSpeed = lowestSpeed;

        var selected2 = Game.Instance.workers.FindAll(s => s.selected);
        rows = Math.Round(Math.Sqrt(selected2.Count), 0);
        xSpace = zSpace = 0;

        foreach (Unit u in selected2)
        {
            rows--;
            if (rows <= 1)
            {
                xSpace = 0;
                zSpace += 5;
                rows = Math.Round(Math.Sqrt(selected2.Count), 0);
            }
            xSpace += 5;
            u.SetDestination(new Vector3(hit.point.x + xSpace, hit.point.y, hit.point.z + zSpace));
        }
    }
}