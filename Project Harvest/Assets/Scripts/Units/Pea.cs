using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pea : Worker
{
    void Update()
    {
        base.Update();
    }

    public override void Spawn()
    {
        Game.Instance.peas.Add(this);
    }

    public override void Destroy()
    {
        Game.Instance.peas.Remove(this);
    }

    public override List<Troop> GetEnemyTroops()
    {
        return Game.Instance.fruits;
    }

    public override void SelectType()
    {
        int selectedCount = 0;
        foreach (Pea p in Game.Instance.peas)
        {
            if (selectedCount > 84)
                break;

            if (p.isDying)
                continue;

            selectedCount++;
            p.ToggleSelected(true);
        }
        Game.Instance.ChangeSelection();
    }
}
