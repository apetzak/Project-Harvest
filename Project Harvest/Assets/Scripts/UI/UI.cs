using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text resourceText;

    private void Update()
    {
        resourceText.text = GetResourceText();
    }

    private string GetResourceText()
    {
        return string.Format("Water {0} || Wood {1} || Gold {2} || Stone {3} || Fertilizer {4}",
            Game.Instance.fruitResourceWater, Game.Instance.fruitResourceWood,
            Game.Instance.fruitResourceGold, Game.Instance.fruitResourceStone,
            Game.Instance.fruitResourceFertilizer);
    }
}
