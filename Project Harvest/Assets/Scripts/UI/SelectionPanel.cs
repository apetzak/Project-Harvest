using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPanel : MonoBehaviour
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
    public List<RawImage> images;
    private Entity entity;
    //private bool change = true;
    private bool visible = true;
    //private bool unitBoxesActive = false;
    //private int selectedCount = 0;

    private void Start()
    {
        rt = lifeBar.GetComponent<RectTransform>();
        green = lifeBar.GetComponent<Image>();
        red = redBar.GetComponent<Image>();
        initPosition = rt.transform.localPosition;
        txt = text.GetComponent<Text>();
        CreateUnitBoxes();
    }

    private void LateUpdate()
    {
        if (entity != null)
            UpdateHealthBar();

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
            unitBoxes[i].image.texture = images[units[i].index].texture;
            var barLength = 10 / units[i].maxHealth * units[i].health * 2.5f;
            var v = unitBoxes[i].green.GetComponent<RectTransform>();
            v.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barLength);
            //v.transform.localPosition = initPosition;
            //v.transform.localPosition += new Vector3(barLength / 2 - 57.5f, 0, 0);
        }
    }

    public void UpdateUnitBoxHealthBar()
    {

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
        foreach (RawImage i in images)
            i.enabled = false;
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
        if (entity is Unit)
        {
            foreach (RawImage i in images)
                i.enabled = false;
            images[(entity as Unit).index].enabled = true;
        }
        txt.enabled = green.enabled = red.enabled = true;
        txt.text = GetText();
    }

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

        if (entity is Troop)
        {
            Troop t = entity as Troop;
            return $"Health        {t.maxHealth}\nSpeed        {t.speed}" +
            $"\nDamage     {t.attackDamage}\nA. Speed    {t.attackSpeed}\nA. Range    {t.attackRange}";
        }
        else if (entity is Worker)
        {
            Worker w = entity as Worker;
            return $"Health        {w.maxHealth}\nSpeed        {w.speed}\nLoad      0/10";
        }
        else if (entity is Farm)
        {
            return $"{entity.name}\nTeam        {team}\nHealth        {entity.maxHealth}"
            + $"\nState        {(entity as Farm).state}";
        }
        else if (entity is Turret)
        {
            Turret t = entity as Turret;
            return $"turret\nTeam        {team}";
        }
        else if (entity is Structure)
        {
            Structure s = entity as Structure;
            return $"{s.name}\nHealth        {s.maxHealth}\nTeam        {team}";
        }
        else if (entity is Gold)
        {
            return "";
        }
        else if (entity is Stone)
        {
            return "";
        }
        else if (entity is Tree)
        {
            return "";
        }

        return "";
    }
}
