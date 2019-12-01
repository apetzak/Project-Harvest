using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Map : MonoBehaviour
{
    void Start()
    {
        var r = gameObject.GetComponent<RectTransform>();
        //r = new Rect(Screen.width - 150, Screen.height - 150, 150, 150);
        Time.timeScale = .0001f;
    }

    void Update()
    {
       
    }

    void OnGUI()
    {
        //Debug.Log(Input.mousePosition);
        DrawUnits();
        DrawBox();
        //1141 300

    }

    void DrawUnits()
    {
        foreach (Unit u in Game.Instance.troops)
        {
            //Debug.Log(transform.position.x + " " + transform.position.y);
            int x = Convert.ToInt32(u.transform.position.x) / 5;
            int z = Convert.ToInt32(u.transform.position.z) / 5;
            Rect r = new Rect(1624 + x, 911 - z, 2, 2);
            GUI.DrawTexture(r, !u.selected ? Texture2D.whiteTexture : Texture2D.normalTexture);
        }
    }

    void DrawBox()
    {
        //Rect r = new Rect(1624 + x, 911 - z, 2, 2);
        //GUI.DrawTexture(r, !u.selected ? Texture2D.whiteTexture : Texture2D.normalTexture);
    }
}
