using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FieldSFC : MonoBehaviour
{
	private static FieldSFC instance;
	private static GameObject player;

	public bool playerChating = false;
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

            if (GameManager.UI.menuOpen == true)
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
        var chatBox = GameManager.Resource.Load<NPCChatBox>("UI/WIndowUI/Chat Box");
        if (player.GetComponent<PlayerInteraction>().nearbyNPC == null)
        {
            Debug.Log("가까운 NPC 없음");
            return;
        }
        else if (player.GetComponent<PlayerInteraction>().nearbyNPC != null && playerChating == false)
        {
            player.GetComponent<PlayerInteraction>().nearbyNPC.StartTalk();

			chatBox.messages = player.GetComponent<PlayerInteraction>().nearbyNPC.chatDetail.stringList;
            GameManager.UI.ShowWindowUI(chatBox);
			playerChating = true;
        }
		else if (player.GetComponent<PlayerInteraction>().nearbyNPC != null && playerChating == true)
		{
			chatBox.ChangedText();
        }
    }
}
