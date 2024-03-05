using System.Collections.Generic;
using TMPro;

public class NPCChatBox : WindowUI
{
    public TextMeshProUGUI chatTextArea;
    public int currentTextIndex;

    public string[] messages;

    private void Start()
    {
        currentTextIndex = 0;
    }

    private void Update()
    {
        chatTextArea.text = messages[currentTextIndex];
    }
}
