using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ItemBuyPopUP : PopUpUI
{
    ShopSlot slot;

    Item target;
    TextMeshProUGUI text;
    TextMeshProUGUI explain;

    protected override void Awake()
    {
        base.Awake();
        buttons["Blocker"].onClick.AddListener(CloseUI);
        buttons["CancleButton"].onClick.AddListener(CloseUI);
        buttons["AcceptButton"].onClick.AddListener(BuyItem);
        text = texts["ItemNameText"];
        explain = texts["Text"];
    }

    private void OnEnable()
    {
        explain.text = "구매하시겠습니까?";
    }

    private void BuyItem()
    {
        if (target != null)
        {
            if(GameManager.Stat.Money < target.Price)
            {
                explain.text = "돈이 부족합니다.";
                return;
            }
            GameManager.Inven.AddItem(target);
            GameManager.Stat.SubMoney(target.Price);
        }
        CloseUI();
    }

    public void Init(ShopSlot slot ,Item target)
    {
        this.slot = slot;
        this.target = target;
        text.text = target.ItemName;
    }
}
