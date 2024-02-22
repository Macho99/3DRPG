using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FieldSFC : MonoBehaviour
{
	private static FieldSFC instance;
	private static GameObject player;
	public static GameObject Player
	{
		get
		{
			if (player == null)
				player = GameObject.FindGameObjectWithTag("Player");
			return player;
		}
	}
	public static FieldSFC Instance
	{
		get { return instance; }
	}

	private void Awake()
	{
		if(instance != null)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
	}

	private void OnDestroy()
	{
		if(instance == this)
		{
			instance = null;
			player = null;
		}
	}

    private void OnOpenMenu(InputValue value)
    {
        if (value.isPressed == true)
        {
            GameManager.UI.menuOpen = !GameManager.UI.menuOpen;

            if (GameManager.UI.menuOpen == false)
            {
				GameManager.UI.ShowPopUpUI(GameManager.UI.menu);
                Cursor.lockState = CursorLockMode.Confined;
                player.GetComponent<PlayerInput>().enabled = false;
            }
            else
            {
                GameManager.UI.ClearPopUpUI();
				Cursor.lockState = CursorLockMode.Locked;
                player.GetComponent<PlayerInput>().enabled = true;
            }
        }
    }

	private void OnInteraction(InputValue value)
	{

	}
}
