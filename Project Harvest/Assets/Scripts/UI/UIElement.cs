using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        CursorSwitcher.Instance.Switch(null);
        Game.Instance.mouseOverUI = true;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Game.Instance.mouseOverUI = false;
    }
}
