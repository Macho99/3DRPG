using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IsTradeAble : MonoBehaviour
{
    public void OpenShopUI()
    {
        Debug.Log("»óÁ¡ ¿ÀÇÂ");
        GameManager.UI.ShowPopUpUI<ShopUI>("UI/PopUpUI/Shop/Shop");
        FieldSFC.Player.IgnoreInput(true);
    }
}
