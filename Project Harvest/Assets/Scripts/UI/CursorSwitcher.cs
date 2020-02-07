using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSwitcher : MonoBehaviour
{
    public static CursorSwitcher Instance { get; } = new CursorSwitcher();

    public List<Texture2D> textures;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        Instance.textures = textures;
    }

    public void Set(int i)
    {
        if (Instance.textures.Count <= i)
            return;

        Cursor.SetCursor(Instance.textures[i], hotSpot, cursorMode);
    }

    public void Set(string s)
    {

    }
}
