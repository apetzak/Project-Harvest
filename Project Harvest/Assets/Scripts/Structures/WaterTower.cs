using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTower : Structure
{
    public bool turnedOn;
    public int waterConsumed = 0;
    public List<Sprinkler> sprinklers;
    private int counter = 60;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (!isBuilt | !turnedOn || sprinklers.Count == 0)
            return;

        if (fruit && Game.Instance.fruitResourceWater <= 0 || !fruit && Game.Instance.veggieResourceWater <= 0)
        {
            turnedOn = false;
            return;
        }

        counter--;

        if (counter <= 0)
        {
            if (fruit)
            {
                if (Game.Instance.fruitResourceWater > sprinklers.Count)
                    Game.Instance.fruitResourceWater -= ConsumeWater();

                if (Game.Instance.fruitResourceWater <= 0)
                    DeactivateSprinklers();
            }
            else
            {
                if (Game.Instance.veggieResourceWater >= sprinklers.Count)
                    Game.Instance.veggieResourceWater -= ConsumeWater();

                if (Game.Instance.veggieResourceWater < sprinklers.Count)
                    DeactivateSprinklers();
            }
        }

        base.Update();
    }

    public int ConsumeWater()
    {
        waterConsumed += sprinklers.Count;
        counter = 60;
        return sprinklers.Count;
    }

    public void ActivateSprinklers()
    {
        turnedOn = fruit && Game.Instance.fruitResourceWater > 0 || !fruit && Game.Instance.veggieResourceWater > 0;

        var list = fruit ? Game.Instance.fruitStructures : Game.Instance.veggieStructures;
        foreach (Structure s in list)
        {
            if (s is Sprinkler && !(s as Sprinkler).hasSource)
            {
                Vector3 diff = transform.position - s.transform.position;
                if (diff.magnitude < 160) // sprinklers in range
                {
                    (s as Sprinkler).turnedOn = turnedOn;
                    (s as Sprinkler).hasSource = true;
                    sprinklers.Add(s as Sprinkler);
                }
            }
        }
    }

    private void DeactivateSprinklers()
    {
        turnedOn = false;

        for (int i = sprinklers.Count - 1; i >= 0; i--)
        {
            sprinklers[i].TurnOff();
            sprinklers.RemoveAt(i);
        }
    }

    public override void Remove()
    {
        DeactivateSprinklers();
        base.Remove();
    }
}
