using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPanel : UIElement
{
    public List<UnitBox> unitBoxes;
    public UnitBox prefab;
    public int index = 0;
    public GameObject lifeBar;
    private Vector3 initPosition;
    private RectTransform rt;
    public GameObject redBar;
    private Image green;
    private Image red;
    public GameObject text;
    private Text txt;
    public GameObject pic;
    public RawImage singleImage;
    public List<Texture2D> images;
    private Entity entity;
    private bool visible = true;

    private void Start()
    {
        rt = lifeBar.GetComponent<RectTransform>();
        green = lifeBar.GetComponent<Image>();
        red = redBar.GetComponent<Image>();
        initPosition = rt.transform.localPosition;
        txt = text.GetComponent<Text>();
        CreateUnitBoxes();
        PopulateImages();
    }

    private void PopulateImages()
    {
        List<string> list = new List<string>();
        list.Add("blueberry");
        list.Add("pea");
        foreach (TroopClass t in TroopClass.Instance.list)
            list.Add(t.name);
        foreach (string s in list)
            images.Add(Assets.GetUnitSprite(s.ToLower()));
    }

    private void LateUpdate()
    {
        if (entity != null)
        {
            UpdateHealthBar();
            txt.text = GetText();
        }

        if (Game.Instance.selectionChanged)
        {
            Game.Instance.selectionChanged = false;

            if (Game.Instance.selectedEntity == null) // && Game.Instance.selectedUnits.Count != 1)
                HideSingle();
            else if (Game.Instance.selectedEntity != entity)
                ShowSingle();

            HideUnitBoxes();

            if (Game.Instance.selectedUnits.Count > 1)
                ShowUnitBoxes();
        }
    }

    /// <summary>
    /// Create grid of unit boxes (5x17), set them as child, hide grid
    /// </summary>
    private void CreateUnitBoxes()
    {
        for (float j = 0; j < 5; j++)
        {
            for (float i = 0; i < 17; i++)
            {
                Vector3 pos = new Vector3(19.7f + i * 35, 163 - j * 35, 0);
                UnitBox u = Instantiate(prefab, pos, Quaternion.identity);
                unitBoxes.Add(u);
                u.transform.parent = transform;
                u.transform.gameObject.SetActive(false);
            }
        }
    }

    private void ShowUnitBoxes()
    {
        var units = new List<Unit>();
        foreach (Unit u in Game.Instance.selectedUnits)
            units.Add(u);

        HideUnitBoxes();

        for (int i = 0; i < Game.Instance.selectedUnits.Count; i++)
        {
            unitBoxes[i].transform.gameObject.SetActive(true);
            unitBoxes[i].image.texture = images[units[i].index];
            var barLength = 10 / units[i].maxHealth * units[i].health * 2.5f;
            var v = unitBoxes[i].green.GetComponent<RectTransform>();
            v.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barLength);
        }
    }

    public void UpdateUnitBoxHealthBar()
    {
        // todo
    }

    private void HideUnitBoxes()
    {
        foreach (UnitBox u in unitBoxes)
            u.transform.gameObject.SetActive(false);
    }

    private void HideSingle()
    {
        if (!txt.enabled)
            return;
        singleImage.enabled = false;
        txt.enabled = green.enabled = red.enabled = false;
        entity = null;
        visible = false;
    }

    /// <summary>
    /// Set entity to new selection, show image, enable health bar, show details
    /// </summary>
    private void ShowSingle()
    {
        entity = Game.Instance.selectedEntity;
        singleImage.enabled = true;

        string entityName = entity.name.Replace("(Clone)", "").ToLower();

        if (entity is Unit)
            singleImage.texture = Assets.GetUnitSprite(entityName);
        else if (entity is BlueberryBush)
            singleImage.texture = Assets.GetStructureSprite("blueberry").texture;
        else if (entity is PeaPlant)
            singleImage.texture = Assets.GetStructureSprite("pea").texture;
        else if (entity is Farm)
            singleImage.texture = Assets.GetStructureSprite((entity as Farm).troop.ToLower()).texture;
        else if (entity is Structure)
            singleImage.texture = Assets.GetStructureSprite(entityName).texture;

        txt.enabled = green.enabled = red.enabled = true;
        txt.text = GetText();
    }

    /// <summary>
    /// Update size of selected target health bar
    /// </summary>
    private void UpdateHealthBar()
    {
        var barLength = 175 / entity.maxHealth * entity.health * 1.15f * .75f;
        var v = lifeBar.GetComponent<RectTransform>();
        v.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barLength);
        v.transform.localPosition = initPosition;
        v.transform.localPosition += new Vector3(barLength / 2 - 75, 0, 0);
    }

    private string GetText()
    {
        string team = entity.fruit ? "Fruit" : "Veggie";
        string text = string.Empty;
        text += $"Name           {entity.name}\n";
        if (!(entity is Resource))
            text += $"Team           {team}\n";
        text += $"Health         {entity.health}/{entity.maxHealth}\n";

        if (entity is Troop)
        {
            Troop t = entity as Troop;
            text += $"Speed         {t.speed}\n";
            text += $"Damage       {t.attackDamage}\n";
            text += $"A. Speed    {t.attackSpeed}\n";
            text += $"Range         {t.attackRange}\n";
        }
        else if (entity is Worker)
        {
            Worker w = entity as Worker;
            text += $"State          {w.state}\n";
            text += $"Load           {w.resourceCount}/{w.resourceCapacity}\n";
        }
        if (entity is Farm)
        {
            Farm f = entity as Farm;
            text += $"State           {f.state}\n";
            text += $"Rally Point   {f.rallyPoint}\n";
            text += $"Grass         {f.grassCount}\n";
        }
        if (entity is Structure)
        {
            Structure s = entity as Structure;
            if (s.ring != null)
                text += $"Range          {s.ring.transform.localScale}\n";

            text += $"{s.minX} {s.maxX} {s.minZ} {s.maxZ}\n";
            text += $"{s.woodCost} {s.stoneCost} {s.goldCost}\n";
        }
        if (entity is Resource)
        {
            text += $"Occupied          {(entity as Resource).occupied}\n";
        }
        if (entity is WaterTower)
        {
            text += $"On                {(entity as WaterTower).turnedOn}\n";
            text += $"Water Consumed    {(entity as WaterTower).waterConsumed}\n";
            text += $"Sprinklers        {(entity as WaterTower).sprinklers.Count}\n";
        }
        else if (entity is Windmill)
        {
            text += $"On                {(entity as Windmill).turnedOn}\n";
            text += $"Water Collected   {(entity as Windmill).waterCollected}\n";
        }
        else if (entity is Sprinkler)
        {
            text += $"On                {(entity as Sprinkler).turnedOn}\n";
            text += $"Has Source   {(entity as Sprinkler).hasSource}\n";
        }
        else if (entity is CompostBin)
        {
            text += $"Is Full      {(entity as CompostBin).isFull}\n";
            text += $"Load      {(entity as CompostBin).load}/{(entity as CompostBin).loadCapacity}\n";
            text += $"Time    {(entity as CompostBin).fertilizeTime}\n";
        }

        return text;
    }
}
