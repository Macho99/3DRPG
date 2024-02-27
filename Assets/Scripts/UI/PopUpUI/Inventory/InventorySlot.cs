using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image iconImage;

    Transform iconParent;
    public SOItem item = null;
    public int amount;
    public TextMeshProUGUI amountText;

    public bool hasItem = false;

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
        GameManager.Inven.focusSlot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highLight.gameObject.SetActive(false);
        GameManager.Inven.focusSlot = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if( item == null) 
        {
            Debug.Log($"Click {gameObject.name}");
        }
        else if (item != null)
        {
            Debug.Log($"Click {item.Name}");
            GameManager.Inven.selectedSlot = this;
        }
    }
}

