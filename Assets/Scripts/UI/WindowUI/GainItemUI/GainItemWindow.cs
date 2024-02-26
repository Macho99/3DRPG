using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GainItemWindow : WindowUI
{
    public TextMeshProUGUI itemName = null;
    public Image itemImage = null;

    private void OnDisable()
    {
        itemName = null;
        itemImage = null;
    }
}
