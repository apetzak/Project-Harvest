using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitBox : MonoBehaviour
{
    public RawImage image;
    public Image green;
    //public 

    void Start()
    {
        
    }

    void Update()
    {

    }

    void OnMouseDown()
    {
        Debug.Log("on click");
    }

    void OnMouseOver()
    {
        Debug.Log("over");

    }

    void OnPointerDown()
    {
        Debug.Log("point");
    }
}
