using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueberry : Worker
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Spawn()
    {
        Game.Instance.blueberries.Add(this);
    }

    public override void Remove()
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
