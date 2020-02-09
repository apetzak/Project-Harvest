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

    /// <summary>
    /// Hide ring if structure is selected, toggle all selectedUnits off, clear selectedUnits
    /// </summary>
    public static void ClearSelection()
    {
        if (Game.Instance.selectedUnit is Structure && 
           (Game.Instance.selectedUnit as Structure).ring != null)
            (Game.Instance.selectedUnit as Structure).ring.GetComponent<MeshRenderer>().enabled = false;

        foreach (Unit u in Game.Instance.selectedUnits)
            u.ToggleSelected(false);
        Game.Instance.selectedUnits = new List<Unit>();
    }

    /// <summary>
    /// Sets destination of selectedUnits to RaycastHit.point (spot clicked on the zone).
    /// Create basic formation.
    /// </summary>
    /// <param name="hit"></param>
    public static void SetGroupLocation(Vector3 point)
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
            u.SetDestination(new Vector3(point.x + xSpace, point.y, point.z + zSpace));
        }

        foreach (Unit u in Game.Instance.selectedUnits)
            u.currentSpeed = lowestSpeed;
    }
}