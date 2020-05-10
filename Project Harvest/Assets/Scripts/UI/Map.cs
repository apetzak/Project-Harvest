using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 
/// </summary>
public class Map : UIElement, IPointerClickHandler
{
    public GameObject viewBox;
    public bool leftButtonDown;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public Texture2D textureFruit;
    public Texture2D textureVeggie;
    public Texture2D textureWood;
    public Texture2D textureGold;
    public Texture2D textureStone;
    private int tickCount = 0;

    private void Start()
    {
        var r = gameObject.GetComponent<RectTransform>();

        Vector3[] corners = new Vector3[4];
        r.GetWorldCorners(corners);
        minX = corners[0].x;
        maxX = corners[2].x;
        minY = Screen.height - corners[0].y;
        maxY = Screen.height - corners[1].y;

        textureFruit = GetTexture("Red");
        textureVeggie = GetTexture("Veggie");
        textureWood = GetTexture("Brown");
        textureGold = GetTexture("Yellow");
        textureStone = GetTexture("Gray");

        foreach (Entity e in Game.Instance.resources)
            DrawResource(e);
    }

    private Texture2D GetTexture(string color)
    {
        Texture2D t = new Texture2D(1, 1);
        t.SetPixel(0, 0, Assets.GetMaterial(color).color);
        t.Apply();
        return t;
    }

    private void OnGUI()
    {
        tickCount++;
        if (tickCount < 2)
            return;
        tickCount = 0;

        if (leftButtonDown)
        {
            if (!Input.GetMouseButtonDown(0))
                leftButtonDown = false;
            else
                MoveCamera(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        }

        DrawObjects();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            MoveCamera(eventData.position);
        else if (eventData.button == PointerEventData.InputButton.Right)
            MoveUnits(eventData.position);
    }

    private void MoveCamera(Vector2 pos)
    {
        leftButtonDown = true;

        // zone 1500 x 1500
        float x = (pos.x - minX) * 7.5f;
        float z = (pos.y * 7.5f) + maxY - minY;
        float y = Camera.main.transform.position.y;
        Camera.main.transform.position = new Vector3(x, y, z);

        MoveViewBox(pos);
    }

    private void MoveViewBox(Vector2 pos) 
    {
        viewBox.transform.position = new Vector3(pos.x, pos.y, 0);
    }

    private void MoveUnits(Vector2 pos)
    {
        float x = (pos.x - minX) * 7.5f;
        float z = (pos.y * 7.5f) + maxY - minY;
        Vector3 point = new Vector3(x, 10, z);
        EntityUtils.SetGroupLocation(Game.Instance.selectedUnits, point);
    }

    private void DrawObjects()
    {
        foreach (Entity e in Game.Instance.fruits)
            DrawEntity(e);
        foreach (Entity e in Game.Instance.veggies)
            DrawEntity(e);
        foreach (Entity e in Game.Instance.blueberries)
            DrawEntity(e);
        foreach (Entity e in Game.Instance.peas)
            DrawEntity(e);
        foreach (Entity e in Game.Instance.fruitStructures)
            DrawEntity(e);
        foreach (Entity e in Game.Instance.veggieStructures)
            DrawEntity(e);
        foreach (Entity e in Game.Instance.fruitFarms)
            DrawEntity(e);
        foreach (Entity e in Game.Instance.veggieFarms)
            DrawEntity(e);
        //foreach (Entity e in Game.Instance.resources)
        //    DrawResource(e);
        foreach (Entity e in Game.Instance.selectedUnits)
            DrawSelected(e);
    }

    private void DrawEntity(Entity e)
    {
        float x = e.transform.position.x / 7.5f;
        float z = e.transform.position.z / 7.5f;
        Rect r = new Rect(minX + x, minY - z, 2, 2);

        //if (e is Unit && !(e as Unit).moving)
        //    return;

        if (e.fruit)
            GUI.DrawTexture(r, textureFruit);
        else
            GUI.DrawTexture(r, textureVeggie);
    }

    private void DrawSelected(Entity e)
    {
        float x = e.transform.position.x / 7.5f;
        float z = e.transform.position.z / 7.5f;
        Rect r = new Rect(minX + x, minY - z, 2, 2);
        GUI.DrawTexture(r, Texture2D.whiteTexture);
    }

    private void DrawResource(Entity e)
    {
        // todo: figure out how to not re-draw still objects
        if (e is Tree)
            return;

        float x = e.transform.position.x / 7.5f;
        float z = e.transform.position.z / 7.5f;
        Rect r = new Rect(minX + x, minY - z, 2, 2);

        if (e is Gold)
            Graphics.DrawTexture(r, textureGold);
        else if (e is Stone)
            Graphics.DrawTexture(r, textureStone);
        else if (e is Tree)
            Graphics.DrawTexture(r, textureWood);
    }
}
