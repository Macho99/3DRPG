using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : PopUpUI
{
    IsTradeAble merchant;
    Item[] items;

    ShopSlot slotPrefab;
    RectTransform itemParents;

    protected override void Awake()
    {
        base.Awake();
        buttons["ExitButton"].onClick.AddListener(CloseUI);
        buttons["CloseButton"].onClick.AddListener(CloseUI);
        itemParents = transforms["ItemContent"];
        slotPrefab = GameManager.Resource.Load<ShopSlot>("UI/PopUpUI/Shop/ShopSlot");
    }

    private void Start()
    {

    }

    private void Update()
    {

    }
}
