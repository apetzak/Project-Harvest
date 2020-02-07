using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Any object that a player can create (units, structures, farms).
/// Can be killed/destroyed.
/// When selected by player, a details panel for this object will appear in UI.
/// </summary>
public class Entity : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public bool isDying = false;
    public bool fruit;
    public bool selected;

    public virtual void Destroy()
    {

    }

    public virtual void Spawn()
    {

    }

    public virtual void ToggleSelected(bool b)
    {

    }
}
