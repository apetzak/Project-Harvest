using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : MonoBehaviour
{
    public Text resourceText;
    public List<Button> buttons = new List<Button>();

    private void Update()
    {
        resourceText.text = GetResourceText();
    }

    private string GetResourceText()
    {
        if (Game.Instance.fruit)
        {
            return string.Format("Water {0} || Wood {1} || Gold {2} || Stone {3} || Fertilizer {4} || Team Fruit",
                Game.Instance.fruitResourceWater, Game.Instance.fruitResourceWood,
                Game.Instance.fruitResourceGold, Game.Instance.fruitResourceStone,
                Game.Instance.fruitResourceFertilizer);
        }
        else
        {
            return string.Format("Water {0} || Wood {1} || Gold {2} || Stone {3} || Fertilizer {4} || Team Veggie",
                Game.Instance.veggieResourceWater, Game.Instance.veggieResourceWood,
                Game.Instance.veggieResourceGold, Game.Instance.veggieResourceStone,
                Game.Instance.veggieResourceFertilizer);
        }
    }
}
