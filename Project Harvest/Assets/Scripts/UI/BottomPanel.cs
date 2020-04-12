using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : UIElement
{
    public Text txtWater;
    public Text txtWood;
    public Text txtGold;
    public Text txtStone;
    public Text txtFertilizer;

    public List<Button> buttons = new List<Button>();

    private void Update()
    {
        if (Game.Instance.fruit)
        {
            txtWater.text = Game.Instance.fruitResourceWater.ToString();
            txtWood.text = Game.Instance.fruitResourceWood.ToString();
            txtGold.text = Game.Instance.fruitResourceGold.ToString();
            txtStone.text = Game.Instance.fruitResourceStone.ToString();
            txtFertilizer.text = Game.Instance.fruitResourceFertilizer.ToString();
        }
        else
        {
            txtWater.text = Game.Instance.veggieResourceWater.ToString();
            txtWood.text = Game.Instance.veggieResourceWood.ToString();
            txtGold.text = Game.Instance.veggieResourceGold.ToString();
            txtStone.text = Game.Instance.veggieResourceStone.ToString();
            txtFertilizer.text = Game.Instance.veggieResourceFertilizer.ToString();
        }
    }
}
