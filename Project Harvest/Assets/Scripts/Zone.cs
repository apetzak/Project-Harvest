using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Zone : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {

    }

    private void OnMouseOver()
    {
        if (Game.Instance.mouseOverUI)
            return;

        if (Input.GetMouseButtonDown(0))
            LeftClick();

        else if (Input.GetMouseButtonDown(1))
            RightClick();

        //else if (Input.GetMouseButtonDown(2)) // middle
    }

    private void LeftClick()
    {
        EntityUtils.ClearSelection();
        Game.Instance.ChangeSelection();
    }

    private void RightClick()
    {
        SetDestinations();
        EntityUtils.ClearTargets();       
    }

    private void SetDestinations()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (!Physics.Raycast(ray, out hit))    
            return;
        EntityUtils.SetGroupLocation(hit.point);
    }

    private void OnMouseDown()
    {

    }

    private void OnMouseEnter()
    {
        CursorSwitcher.Instance.Switch(null);
    }
}
