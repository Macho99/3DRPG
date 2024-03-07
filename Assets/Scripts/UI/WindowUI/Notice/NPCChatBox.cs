using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;

public class NPCChatBox : WindowUI
{
    public TextMeshProUGUI dialogueText;
    public GameObject nextText;

    [HideInInspector] public Queue<string> sentenceLines = new();

    private string currentText;

    public bool isTyping;

    protected override void Awake()
    {
        base.Awake();
    }

    public void OnDialogue(string[] getLines)
    {
        sentenceLines.Clear();
        foreach (string line in getLines)
        {
            sentenceLines.Enqueue(line);
        }
    }

    public void NextSentence()
    {
        if (sentenceLines.Count != 0)
        {
            GameManager.Dialogue.InteractionNPC.GetComponent<Animator>().SetTrigger("IsTalk");
            currentText = sentenceLines.Dequeue();
            nextText.SetActive(false);
            isTyping = true;
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(Typing(currentText));
            }
        }
        else
        {
            GameManager.UI.ClearWindowUI();
            if(GameManager.Dialogue.InteractionNPC.GetComponent<IsTradeAble>() == true)
            {
                GameManager.Dialogue.InteractionNPC.GetComponent<IsTradeAble>().OpenShopUI();
            }
        }
    }

    public IEnumerator Typing(string line)
    {
        print("typeΩ√¿€");
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void Update()
    {
        if (dialogueText.text.Equals(currentText))
        {
            isTyping = false;
            nextText.SetActive(true);
        }
    }
}
