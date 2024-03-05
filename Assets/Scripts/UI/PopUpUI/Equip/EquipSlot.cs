using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image iconImage;

    Transform iconParent;
    internal SOItem item = null;

    public Image highLight;

    private void Awake()
    {
        iconParent = iconImage.rectTransform.parent;
    }

    public virtual bool TrySetItem(SOItem item)
    {
        if (item == null) { return false; }
        this.item = item;
        iconImage.enabled = true;
        iconImage.sprite = item.Icon;
        return true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highLight.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highLight.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null)
        {
            Debug.Log($"Click {gameObject.name}");
        }
        else if (item != null)
        {
            Debug.Log($"Click {item.Name}");
        }
    }
}
