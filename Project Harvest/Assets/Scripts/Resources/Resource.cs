using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Entity
{
    public bool occupied;
    public GameObject prop;
    public int stage = 0;

    protected virtual void Start()
    {
        float size = Random.Range(.75f, 1.25f);
        transform.Rotate(0, Random.Range(0, 360), 0);
        transform.localScale *= size;
        maxHealth = health = size * 1000;
    }

    void Update()
    {
        
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            LeftClick();
        else if (Input.GetMouseButtonDown(1))
            RightClick();
    }

    protected virtual void LeftClick()
    {

    }

    protected virtual void RightClick()
    {

    }

    public void GatherFrom()
    {

    }

    protected void Shrink()
    {
        stage++;
        transform.localScale *= .85f;

        if (stage == 5)
            Destroy(gameObject);
    }
}
