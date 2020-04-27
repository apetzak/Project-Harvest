using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cursor
{
    Default,
    Sword,
    Plant,
    WaterDrop,
    Can,
    Shovel,
    Pickaxe,
    Hammer,
    Axe,
    Rake
}

public class CursorSwitcher : MonoBehaviour
{
    public static CursorSwitcher Instance { get; } = new CursorSwitcher();

    public List<Texture2D> textures;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;
    private cursor current;

    void Start()
    {
        Instance.textures = textures;
        Switch(null);
    }

    /// <summary>
    /// Change cursor image. Pass null for default.
    /// </summary>
    /// <param name="e"></param>
    public void Switch(Entity e)
    {
        if (e is null || e.isDying)
        {
            current = cursor.Default;
        }
        else if (e is Resource)
        {
            if (Game.Instance.workerIsSelected)
            {
                if (e is Tree)
                    current = cursor.Axe;
                else if (e is Water)
                    current = cursor.WaterDrop;
                else // gold or stone
                    current = cursor.Pickaxe;
            }
        }
        else if (!e.IsAlly() && Game.Instance.troopIsSelected)
        {
            current = cursor.Sword;
        }
        else if (e.IsAlly())
        {
            if (Game.Instance.workerIsSelected)
            {
                if (e is Structure)
                {
                    if (e is Farm)
                    {
                        Farm f = e as Farm;
                        if (f.state == Farm.State.Empty)
                            current = cursor.Plant;

                        else if (f.state == Farm.State.Grassy)
                            current = cursor.Shovel;

                        else if (f.state == Farm.State.Planting || f.state == Farm.State.Growing
                              || f.state == Farm.State.PlantGrowing || f.state == Farm.State.Sprouting)
                            current = cursor.Can;

                        else if (f.state == Farm.State.Dead || f.state == Farm.State.Pickable)
                            current = cursor.Shovel;
                    }
                    else if (e.health < e.maxHealth)
                    {
                        current = cursor.Hammer;
                    }
                }
                else if (e is Unit)
                {
                    // todo: waggoning
                }
            }
            else
            {
                current = cursor.Default;
            }
        }

        Cursor.SetCursor(Instance.textures[(int)current], hotSpot, cursorMode);
    }
}