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

    //void Update()
    //{

    //}

    public virtual void LeftClick()
    {

    }

    public void RightClick()
    {

    }

    int i = 0;

    protected virtual void OnMouseEnter()
    {
        CursorSwitcher.Instance.Set(i++);
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            LeftClick();
        else if (Input.GetMouseButtonDown(1))
            RightClick();
    }

}
