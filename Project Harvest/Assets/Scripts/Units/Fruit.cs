using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : Troop
{
    public override void Spawn()
    {
        Game.Instance.fruits.Add(this);
    }

    public override void Remove()
    {
        Game.Instance.fruits.Remove(this);
        Destroy(gameObject);
    }

    public override List<Troop> GetAllyTroops()
    {
        return Game.Instance.fruits;
    }

    public override List<Troop> GetEnemyTroops()
    {
        return Game.Instance.veggies;
    }

    public override List<Worker> GetEnemyWorkers()
    {
        return Game.Instance.peas;
    }

    public override List<Structure> GetEnemyStructures()
    {
        return Game.Instance.veggieStructures;
    }

    public override List<Farm> GetEnemyFarms()
    {
        return Game.Instance.veggieFarms;
    }

    public override void SelectType()
    {
        int selectedCount = 0;
        foreach (Fruit f in Game.Instance.fruits)
        {
            if (selectedCount > 84)
                break;

            if (f.index != index || f.isDying)
                continue;

            selectedCount++;
            f.ToggleSelected(true);
        }
        Game.Instance.ChangeSelection();
    }
}
