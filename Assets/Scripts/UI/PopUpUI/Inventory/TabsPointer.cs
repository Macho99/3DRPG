using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabsPointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image backgroundImg;
    Color currentColor;

    private void Awake()
    {
        currentColor = backgroundImg.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backgroundImg.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backgroundImg.color = currentColor;
    }
}
