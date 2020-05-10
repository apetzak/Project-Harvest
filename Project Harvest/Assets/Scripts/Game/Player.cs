using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// todo
/// </summary>
public class Player : MonoBehaviour
{
    public bool fruit;
    public List<Troop> troops { get; set; }
    public List<Worker> workers { get; set; }
    public List<Structure> structures { get; set; }
    public int resourceWater;
    public int resourceWood;
    public int resourceStone;
    public int resourceGold;
}
