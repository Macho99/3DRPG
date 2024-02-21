using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUseUI : MonoBehaviour
{
    bool menuOpen = true;

    private void OnOpenMenu(InputValue value)
    {
        if(value.isPressed == true)
        {
            menuOpen = !menuOpen;

            if (menuOpen == false)
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
