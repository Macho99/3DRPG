using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUseUI : MonoBehaviour
{
    private void OnOpenMenu(InputValue value)
    {
        if(value.isPressed == true)
        {
            GameManager.UI.menuOpen = !GameManager.UI.menuOpen;

            if (GameManager.UI.menuOpen == false)
            {
                GameManager.UI.ShowPopUpUI(GameManager.UI.menu);
                gameObject.GetComponent<PlayerLook>().enabled = false;
            }
            else
            {
                GameManager.UI.ClearPopUpUI();
                gameObject.GetComponent<PlayerLook>().enabled = true;
            }
        }
    }
}
