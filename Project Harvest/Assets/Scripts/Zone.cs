using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Zone : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {

    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            LeftClick();

        else if (Input.GetMouseButtonDown(1))
            RightClick();

        else if (Input.GetMouseButtonDown(2)) // middle
        {

        }
    }

    void LeftClick()
    {
        UnitUtils.ClearSelection();
        Game.Instance.ChangeSelection();
    }

    void RightClick()
    {
        SetDestinations();
        UnitUtils.ClearTargets();       
    }

    void SetDestinations()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (!Physics.Raycast(ray, out hit))    
            return;
        UnitUtils.SetGroupLocation(hit);
    }

    void OnMouseDown()
    {

    }

    void OnMouseEnter()
    {
        CursorSwitcher.Instance.Set(0);
    }
}
