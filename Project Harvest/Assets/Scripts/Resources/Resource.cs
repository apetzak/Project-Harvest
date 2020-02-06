using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public float amount;
    public float startingAmount;
    public bool occupied = false;
    public GameObject prop;

    protected virtual void Start()
    {
        transform.Rotate(0, Random.Range(0, 360), 0);
        transform.localScale *= Random.Range(.75f, 1.25f);
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
}
