using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    ShopUI shopUI;

    public string itemName;
    public Item curItem;

    public Image itemImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemSummaryText;
    public TextMeshProUGUI itemPrice;

    private void Start()
    {
        curItem = GameManager.Data.GetItem(itemName);
    }

    private void Update()
    {
        if (curItem != null)
        {
            itemImage.sprite = curItem.Sprite;
            itemNameText.text = curItem.ItemName;
            itemSummaryText.text = curItem.Summary;
            itemPrice.text = curItem.Price.ToString();
        }
        else
        {
            itemImage = null;
            itemNameText.text = "-";
            itemSummaryText.text = "-";
            itemPrice.text = "-";
        }
    }

    public void CheckBuyItem()
    {
        GameManager.UI.ShowPopUpUI<ItemBuyPopUP>("UI/PopUpUI/Shop/ItemBuyPopUp", false).Init(this, curItem);
    }
}
