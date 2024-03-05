using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void OnDestroy()
    {
        if (instance == this)
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
                player.IgnoreInput(true);
            }
            else
            {
                GameManager.UI.ClearPopUpUI();
                player.IgnoreInput(false);
            }
        }
    }


    private void OnInteraction(InputValue value)
	{
        if(player.GetComponent<PlayerInteraction>().nearbyNPC != null)
        {
            //var newChatBox = player.GetComponent<PlayerInteraction>().nearbyNPC.chatBox;
            if (GameManager.Pool.IsContain(player.GetComponent<PlayerInteraction>().nearbyNPC.chatBox) == true)
            {
                if(player.GetComponent<PlayerInteraction>().nearbyNPC.chatBox.currentTextIndex < player.GetComponent<PlayerInteraction>().nearbyNPC.chatBox.messages.Length)
                {
                    player.GetComponent<PlayerInteraction>().nearbyNPC.chatBox.currentTextIndex++;
                }
                else
                {
                    GameManager.UI.ClearWindowUI();
                    player.GetComponent<PlayerInteraction>().nearbyNPC.chatBox.currentTextIndex = 0;
                }
            }
            else
            {
                GameManager.UI.ShowWindowUI(player.GetComponent<PlayerInteraction>().nearbyNPC.chatBox);
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
