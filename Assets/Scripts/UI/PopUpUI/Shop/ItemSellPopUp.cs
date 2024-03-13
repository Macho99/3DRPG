using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSellPopUp : PopUpUI
{
    Item target;
    TextMeshProUGUI nameText;
    TextMeshProUGUI priceText;

    protected override void Awake()
    {
        base.Awake();
        buttons["CloseButton"].onClick.AddListener(CloseUI);
        buttons["Blocker"].onClick.AddListener(CloseUI);
        buttons["CancleButton"].onClick.AddListener(CloseUI);
        buttons["AcceptButton"].onClick.AddListener(SellItem);
        priceText = texts["ItemPrice"];
        nameText = texts["ItemName"];
    }

    private void SellItem()
    {
        if (target != null)
        {
            GameManager.Inven.DeleteItem(target);
            if(target is MultipleItem multipleItem)
			{
				GameManager.Stat.AddMoney(target.Price * multipleItem.Amount);
			}
            else
            {
                GameManager.Stat.AddMoney(target.Price);
            }
        }
        CloseUI();
    }

    public void Init(Item target)
    {
        this.target = target;
        nameText.text = target.ItemName;
        priceText.text = $"{target.Price.ToString()} G";
    }
}
