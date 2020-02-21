using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// todo
/// </summary>
public class User : Player
{
    public static int screenHeight;
    public static int screenWidth;
    public GameObject selectorBox;
    public bool holdingDown = false;
    public Vector3 boxPoint;
    public bool selectionChanged = false;
    public bool workerIsSelected = false;
    public bool troopIsSelected = false;
    public List<Unit> selectedUnits { get; set; }
    public List<Structure> selectedStructures { get; set; }
    public Entity selectedEntity;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
