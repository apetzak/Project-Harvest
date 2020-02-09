using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Veggie : Troop
{
    public override void Spawn()
    {
        Game.Instance.veggies.Add(this);
    }

    public override void Remove()
    {
        Game.Instance.veggies.Remove(this);
        Destroy(gameObject);
    }

    public override List<Troop> GetAllyTroops()
    {
        return Game.Instance.veggies;
    }

    public override List<Troop> GetEnemyTroops()
    {
        return Game.Instance.fruits;
    }

    public override List<Worker> GetEnemyWorkers()
    {
        return Game.Instance.blueberries;
    }

    public override void SelectType()
    {
        int selectedCount = 0;
        foreach (Unit v in Game.Instance.veggies)
        {
            if (selectedCount > 84)
                break;

            if (v.index != index || v.isDying)
                continue;

            selectedCount++;
            v.ToggleSelected(true);
        }
        Game.Instance.ChangeSelection();
    }
}
