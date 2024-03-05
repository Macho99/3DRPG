using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class NoticeUI : PopUpUI
{
    private TextMeshProUGUI noticeText;

    public string notice = null;

    private void OnEnable()
    {
        noticeText = GameObject.Find("NoticeText").transform.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        noticeText.text = notice;
    }
}
