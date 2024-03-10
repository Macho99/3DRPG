using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FieldSFC : MonoBehaviour
{
	[SerializeField] MMF_Player deathfault;
	[SerializeField] MMF_Player attackFail;
	[SerializeField] ChargeFeedback charge;
	[SerializeField] MMF_Player katanaUlti;
	[SerializeField] MMF_Player bowUlti;
	[SerializeField] MMF_Player hit;
	[SerializeField] MMF_Player stun;

	private static FieldSFC instance;
	private static Player player;
	public static Player Player
	{
		get
		{
			if (player == null)
				player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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

	private void Start()
	{
		GameManager.UI.ShowSceneUI<PlayerConditionUI>("UI/SceneUI/PlayerConditionUI");
		GameManager.UI.ShowSceneUI<QuickSlotUI>("UI/SceneUI/QuickSlotUI");
		GameManager.UI.ShowSceneUI<LeftMoneyUI>("UI/SceneUI/Money");
	}

	private void OnDestroy()
	{
		if(instance == this)
		{
			GameManager.UI.ClearSceneUI();
			instance = null;
			player = null;
		}
	}

    private void OnOpenMenu(InputValue value)
    {
		if (value.isPressed == true)
		{
			GameManager.UI.MenuToggle();
		}
	}

	private void OnInteraction(InputValue value)
	{
		if (GameManager.Dialogue.canTalk == true)
		{
			if (GameManager.Dialogue.isTalking == false)
				GameManager.Dialogue.ShowChatBox();
			else
			{
				if (FindObjectOfType<NPCChatBox>() &&
					FindObjectOfType<NPCChatBox>().isTyping == false)
				{
                    FindObjectOfType<NPCChatBox>().NextSentence();
                }
			}
		}
		
    }
	
	public void PlayDeathfault()
	{
		deathfault.PlayFeedbacks();
	}

	public void PlayAttackFail()
	{
		attackFail.PlayFeedbacks();
	}

	public void PlayCharge(bool value)
	{
		if (value == true)
			charge.Play();
		else
			charge.Stop();
	}

	public void PlayCharge(int level)
	{
		charge.SetChargeLevel(level);
	}

	public void PlayKatanaUlti()
	{
		katanaUlti.PlayFeedbacks();
	}

	public void PlayBowUlti()
	{
		bowUlti.PlayFeedbacks();
	}

	public void PlayHit()
	{
		hit.PlayFeedbacks();
	}

	public void PlayStun()
	{
		stun.PlayFeedbacks();
	}
}
