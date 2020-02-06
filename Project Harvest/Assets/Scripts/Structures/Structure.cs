using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Structure : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float minX;
    public float maxX;
    public float maxY;
    public float minY;

    void Start()
    {

    }

    protected virtual void LeftClick()
    {

    }

    protected virtual void RightClick()
    {

    }

    protected virtual void OnMouseEnter()
    {
        CursorSwitcher.Instance.Set(1);
    }

    protected void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            LeftClick();
        else if (Input.GetMouseButtonDown(1))
            RightClick();
    }
}
