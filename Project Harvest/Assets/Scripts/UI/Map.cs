using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour, IPointerClickHandler
{
    public GameObject viewBox;
    public bool leftButtonDown;

    void Start()
    {
        var r = gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        //if (leftButtonDown)
        //{
        //    if (!Input.GetMouseButtonDown(0))
        //        leftButtonDown = false;
        //    else
        //        MoveCamera(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        //}
    }

    void OnGUI()
    {
        if (leftButtonDown)
        {
            if (!Input.GetMouseButtonDown(0))
                leftButtonDown = false;
            else
                MoveCamera(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        }

        DrawUnits();
        //1141 300
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log(eventData.position);
        if (eventData.button == PointerEventData.InputButton.Left)
            MoveCamera(eventData.position);
        else if (eventData.button == PointerEventData.InputButton.Right)
            MoveUnits(eventData);
    }

    private void MoveCamera(Vector2 pos)
    {
        leftButtonDown = true;

        // zone 1500 x 1500
        // map 300 x 300 (0 - 300 y, 1620 - 1920 x)
        float x = (pos.x - 1620) * 5;
        float z = (pos.y * 5) - 50;
        float y = Camera.main.transform.position.y;
        Camera.main.transform.position = new Vector3(x, y, z);

        MoveViewBox(pos);
    }

    private void MoveViewBox(Vector2 pos) 
    {
        viewBox.transform.position = new Vector3(pos.x, pos.y, 0);
    }

    private void MoveUnits(PointerEventData data)
    {
        float x = (data.position.x - 1620) * 5;
        float z = data.position.y * 5;
        Vector3 point = new Vector3(x, 10, z);
        UnitUtils.SetGroupLocation(point);
    }

    void DrawUnits()
    {
        foreach (Entity e in Game.Instance.fruits)
            DrawEntity(e);
        foreach (Entity e in Game.Instance.veggies)
            DrawEntity(e);
        foreach (Entity e in Game.Instance.blueberries)
            DrawEntity(e);
        foreach (Entity e in Game.Instance.peas)
            DrawEntity(e);
    }

    private void DrawEntity(Entity e)
    {
        float x = e.transform.position.x / 5;
        float z = e.transform.position.z / 5;
        Rect r = new Rect(1620 + x, 911 - z, 2, 2);
        GUI.DrawTexture(r, !e.selected ? Texture2D.whiteTexture : Texture2D.normalTexture);
    }
}
