using System.Collections.Generic;
using TMPro;

public class NPCChatBox : WindowUI
{
    public TextMeshProUGUI chatTextArea;
    public int currentTextIndex = 0;

    public List<string> messages;

    private void Update()
    {
        if (messages == null)
            return;
        else
        {
            chatTextArea.text = messages[currentTextIndex];
        }
    }

    public void ChangedText()
    {
        if (currentTextIndex < messages.Count)
        {
            currentTextIndex++;
        }
        else
        {
            GameManager.UI.ClearWindowUI();
        }
    }
}
