using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountingItemsUI : InGameUI
{
    private void Update()
    {
        if (GameManager.UI.menuOpen == false)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0f;
        }
        else if (GameManager.UI.menuOpen == true)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }

    protected override void Init()
    {
        throw new System.NotImplementedException();
    }
}
