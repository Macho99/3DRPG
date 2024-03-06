using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public NPCChatBox chatBoxPrefab;

    public InteractionNPC InteractionNPC;
    public bool canTalk;
    public bool isTalking;

    private void Start()
    {
        chatBoxPrefab = GameManager.Resource.Load<NPCChatBox>("UI/WIndowUI/ChatBox");
        canTalk = false;
        isTalking = false;
    }

    private void Update()
    {
        if(InteractionNPC != null)
        {
            canTalk = true;
        }
        else
        {
            canTalk = false;
        }
    }

    public void ShowChatBox()
    {
        GameManager.UI.ShowWindowUI(chatBoxPrefab);
        FindObjectOfType<NPCChatBox>().OnDialogue(InteractionNPC.sentence);
        FindObjectOfType<NPCChatBox>().NextSentence();
        GameManager.Dialogue.isTalking = true;
    }
}
