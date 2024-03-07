using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTradeAble : MonoBehaviour
{
    public void OpenShopUI()
    {
        Debug.Log("»óÁ¡ ¿ÀÇÂ");
        GameManager.UI.ShowWindowUI<ShopUI>("UI/WIndowUI/Shop/Shop");
        FieldSFC.Player.IgnoreInput(true);
    }
}
