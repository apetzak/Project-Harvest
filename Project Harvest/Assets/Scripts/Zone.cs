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
        Game.Instance.selectionCount = 0;
        Game.Instance.selectionChanged = true;
        Game.Instance.selectedUnit = null;
        Units.DisableAllSelectors();
    }

    void RightClick()
    {
        //Debug.Log("right click");
        SetDestinations();
        Units.ClearTargets();       
    }

    void SetDestinations()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (!Physics.Raycast(ray, out hit))    
            return;
        Units.SetGroupLocation(hit);
    }

    void OnMouseDown()
    {

    }
}
