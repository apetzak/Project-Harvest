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
    /// <summary>
    /// The object that indicates whether an entity is selected or selectable. Appears under the entity (ring or square).
    /// </summary>
    public GameObject selector;
    public float health;
    public float maxHealth;
    public bool isDying = false;
    public bool fruit;
    public bool selected;
    public string description;
    protected int clickTimer;
    protected bool clickedOnce = false;

    /// <summary>
    /// Destroy if health is below zero and not dying
    /// </summary>
    public virtual void TakeDamage()
    {
        if (health <= 0 && !isDying)
        {
            isDying = true;
            Remove();
        }
    }

    /// <summary>
    /// EMPTY
    /// </summary>
    public virtual void Remove()
    {
        //Destroy(gameObject);
    }

    /// <summary>
    /// EMPTY
    /// </summary>
    public virtual void Spawn()
    {

    }

    /// <summary>
    /// Set selected, enabled/disable selector mesh renderer
    /// </summary>
    /// <param name="b"></param>
    public virtual void ToggleSelected(bool b)
    {
        selector.GetComponentInChildren<MeshRenderer>().enabled = selected = b;
    }

    /// <summary>
    /// Select all ally entities of the same type on double click. Must override.
    /// </summary>
    public virtual void SelectType()
    {

    }

    /// <summary>
    /// Call left or right click on mouse down, handle double clicks.
    /// </summary>
    protected virtual void OnMouseOver()
    {
        if (isDying || Game.Instance.mouseOverUI)
            return;

        if (Input.GetMouseButtonDown(0))
            LeftClick();
        else if (Input.GetMouseButtonDown(1))
            RightClick();

        if (clickedOnce)
            clickTimer++;

        if (clickTimer == 20) // cancel double click after 20 frames
        {
            clickTimer = 0;
            clickedOnce = false;
        }
    }

    /// <summary>
    /// ClearSelection, select, select type on double click, change selection
    /// </summary>
    protected virtual void LeftClick()
    {
        if (Game.Instance.mouseOverUI)
            return;

        EntityUtils.ClearSelection();

        if (clickedOnce == true) // double click
        {
            SelectType();
        }
        else
        {
            ToggleSelected(true);
            clickedOnce = true;
        }
        Game.Instance.ChangeSelection();
    }

    /// <summary>
    /// EMPTY
    /// </summary>
    protected virtual void RightClick()
    {

    }

    public bool IsAlly()
    {
        return fruit == Game.Instance.fruit;
    }

    /// <summary>
    /// If not dying and !mouseOverUI, switch cursor
    /// </summary>
    protected virtual void OnMouseEnter()
    {
        if (isDying || Game.Instance.mouseOverUI)
            return;
        CursorSwitcher.Instance.Switch(this);
    }

    /// <summary>
    /// EMPTY
    /// </summary>
    protected virtual void OnMouseExit()
    {
        //CursorSwitcher.Instance.Switch(null);
    }
}
