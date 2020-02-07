using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueberry : Worker
{
    void Start()
    {
        transform.Rotate(0, 90, 0, Space.Self);
        base.Start();
    }

    void Update()
    {
        base.Update();
    }

    public override void Spawn()
    {
        Game.Instance.blueberries.Add(this);
    }

    public override void Destroy()
    {
        Game.Instance.blueberries.Remove(this);
    }

    public override List<Troop> GetEnemyTroops()
    {
        return Game.Instance.veggies;
    }

    public override void SelectType()
    {
        int selectedCount = 0;
        foreach (Blueberry b in Game.Instance.blueberries)
        {
            if (selectedCount > 84)
                break;

            if (b.isDying)
                continue;

            selectedCount++;
            b.ToggleSelected(true);
        }
        Game.Instance.ChangeSelection();
    }
}
