using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorBox : MonoBehaviour
{
    public GameObject selectorBox;
    public bool holdingDown = false;
    public Vector3 start;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !holdingDown)
            OpenPanel();

        if (holdingDown)
            DragPanel();

        if (Input.GetMouseButtonUp(0))
            SelectUnits();
    }

    void OpenPanel()
    {
        holdingDown = true;
        start = Input.mousePosition;
        selectorBox.transform.position = Input.mousePosition;
    }

    void DragPanel()
    {
        var rt = selectorBox.GetComponent<RectTransform>();
        float xDiff = start.x - Input.mousePosition.x;
        float yDiff = start.y - Input.mousePosition.y;
        rt.transform.localScale = new Vector3(xDiff / 234, yDiff / 64);
        rt.transform.position = new Vector3(start.x - xDiff / 2, start.y - yDiff / 2);
    }

    void SelectUnits()
    {
        //UnitUtils.ClearUnitSelection();

        holdingDown = false;
        var rt = selectorBox.GetComponent<RectTransform>();
        var end = Input.mousePosition;
        float maxX = end.x > start.x ? end.x : start.x;
        float minX = end.x < start.x ? end.x : start.x;
        float maxY = end.y > start.y ? end.y : start.y;
        float minY = end.y < start.y ? end.y : start.y;
        int selectedCount = 0;

        if (maxX - minX < 5)
            return;

        foreach (Troop t in Game.Instance.fruits)
        {
            var v = Camera.main.WorldToScreenPoint(t.transform.position);

            if (selectedCount > 84)
                break;

            if (v.x < maxX && v.x > minX && v.y < maxY && v.y > minY && !t.isDying && !t.selected)
            {
                t.ToggleSelected(true);
                selectedCount++;
            }
        }

        foreach (Troop t in Game.Instance.veggies)
        {
            var v = Camera.main.WorldToScreenPoint(t.transform.position);

            if (selectedCount > 84)
                break;

            if (v.x < maxX && v.x > minX && v.y < maxY && v.y > minY && !t.isDying && !t.selected)
            {
                t.ToggleSelected(true);
                selectedCount++;
            }
        }

        foreach (Worker w in Game.Instance.blueberries)
        {
            var v = Camera.main.WorldToScreenPoint(w.transform.position);

            if (selectedCount > 84)
                break;

            if (v.x < maxX && v.x > minX && v.y < maxY && v.y > minY && !w.isDying && !w.selected)
            {
                w.ToggleSelected(true);
                selectedCount++;
            }
        }

        foreach (Worker w in Game.Instance.peas)
        {
            var v = Camera.main.WorldToScreenPoint(w.transform.position);

            if (selectedCount > 84)
                break;

            if (v.x < maxX && v.x > minX && v.y < maxY && v.y > minY && !w.isDying && !w.selected)
            {
                w.ToggleSelected(true);
                selectedCount++;
            }
        }

        Game.Instance.ChangeSelection();
        rt.transform.localScale = new Vector3(0, 0, 0);
    }
}
