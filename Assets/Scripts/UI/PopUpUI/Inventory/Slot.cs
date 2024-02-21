using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
     public Image highLight;

    private void Awake()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highLight.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highLight.gameObject.SetActive(false);
    }
}
