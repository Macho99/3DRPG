using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "new DialogueContainer", menuName = "NPC/DialogueContainer")]
public class DialogueContainer : ScriptableObject
{
    public List<string> stringList;
}

public class InteractionNPC : MonoBehaviour
{
    public DialogueContainer dialogue;
    public bool isInteraction = false;
    public NPCChatBox chatBox;

    private void Start()
    {
        isInteraction = false;
        chatBox = GameManager.Resource.Load<NPCChatBox>("UI/WIndowUI/ChatBox");
        chatBox.messages.Length.Equals(dialogue.stringList.Count);
        chatBox.messages = dialogue.stringList.ToArray();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isInteraction = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            isInteraction = false;
        }
    }
}
