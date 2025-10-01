using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action OnShowTooltip;
    public event Action OnHideTooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnShowTooltip?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHideTooltip?.Invoke();
    }

   
}
