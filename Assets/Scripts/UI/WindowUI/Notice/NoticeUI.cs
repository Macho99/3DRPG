using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class NoticeUI : WindowUI
{
    public TextMeshProUGUI noticeText;

    public string notice;

    private void OnEnable()
    {
        noticeText = GameObject.Find("NoticeText").GetComponent<TextMeshProUGUI>();
        noticeText.text = notice;
    }
}
