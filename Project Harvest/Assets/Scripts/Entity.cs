﻿using System.Collections;
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
    //public bool fruit;
    public bool selected;

    /// <summary>
    /// Destroy if health is below zero
    /// </summary>
    public virtual void TakeDamage()
    {
        if (health <= 0 && !isDying)
        {
            isDying = true;

            if (this is Structure)
                Audio.Instance.PlayExplosion();

            Destroy(gameObject);
        }
    }

    public virtual void Remove()
    {

    }

    public virtual void Spawn()
    {

    }

    public virtual void ToggleSelected(bool b)
    {

    }
}
