using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPanel : MonoBehaviour
{
    //public Material matRed;
    //public Material matGreen;
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
    private Unit unit;
    private bool change = true;
    private bool visible = true;
    private bool unitBoxesActive = false;
    private int selectedCount = 0;


    void Start()
    {
        rt = lifeBar.GetComponent<RectTransform>();
        green = lifeBar.GetComponent<Image>();
        red = redBar.GetComponent<Image>();
        initPosition = rt.transform.localPosition;
        txt = text.GetComponent<Text>();
        CreateUnitBoxes();
    }

    void LateUpdate()
    {
        if (Game.Instance.selectionChanged)
        {
            //Debug.Log("selectionChanged");
            Game.Instance.selectionChanged = false;

            if (Game.Instance.selectedUnit == null && Game.Instance.selectionCount != 1)
                HideSingle();
            else if (Game.Instance.selectedUnit != unit)
                ShowSingle();

            HideUnitBoxes();

            if (Game.Instance.selectionCount > 1)
                ShowUnitBoxes();

            //else if (unit != null)
            //    UpdateHealthBar();

        }
    }

    private void CreateUnitBoxes()
    {
        for (float j = 0; j < 5; j++)
        {
            for (float i = 0; i < 17; i++)
            {
                Vector3 pos = new Vector3(19.7f + i * 35, 163 - j * 35, 0);
                //prefab.transform.SetParent(transform);
                UnitBox u = Instantiate(prefab, pos, Quaternion.identity);
                unitBoxes.Add(u);
                u.transform.parent = transform;
                //u.transform.SetParent(this.transform);
                u.transform.gameObject.SetActive(false);
            }
        }
    }

    private void ShowUnitBoxes()
    {
        //Debug.Log("show unit boxes");
        var units = new List<Unit>();
        var indexs = new List<int>();
        foreach (Troop t in Game.Instance.troops)
        {
            if (!t.selected || t.dying)
                continue;
            units.Add(t);
            indexs.Add(Game.Instance.troops.IndexOf(t));
        }

        HideUnitBoxes();

        for (int i = 0; i < Game.Instance.selectionCount; i++)
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

    private void HideUnitBoxes()
    {
        foreach (UnitBox u in unitBoxes)
            u.transform.gameObject.SetActive(false);
    }

    private void HideSingle()
    {
        //Debug.Log("hide single " + Game.Instance.selectedUnit + " " + Game.Instance.selectionCount);
        if (!txt.enabled)
            return;
        foreach (RawImage i in images)
            i.enabled = false;
        txt.enabled = green.enabled = red.enabled = false;
        unit = null;
        visible = false;
    }

    private void ShowSingle()
    {
        //Debug.Log("show single " + Game.Instance.selectedUnit + " " + Game.Instance.selectionCount);
        unit = Game.Instance.selectedUnit;
        foreach (RawImage i in images)
            i.enabled = false;
        images[unit.index].enabled = true;
        txt.enabled = green.enabled = red.enabled = true;
        txt.text = GetText();
        //foreach (UnitBox u in unitBoxes)
        //    u.transform.gameObject.SetActive(false);
    }

    private void UpdateHealthBar()
    {
        var barLength = 175 / unit.maxHealth * unit.health * 1.15f * .75f;
        var v = lifeBar.GetComponent<RectTransform>();
        v.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barLength);
        v.transform.localPosition = initPosition;
        v.transform.localPosition += new Vector3(barLength / 2 - 75, 0, 0);
    }

    private string GetText()
    {
        Troop t = unit as Troop;
        string s = string.Format(
            "Health        {0}\nSpeed        {1}\n" +
            "Damage     {2}\nA. Speed    {3}\n" +
            "A. Range    {4}",
            t.maxHealth, t.speed, t.attackDamage, t.attackSpeed, t.attackRange);
        return s;
    }
}
