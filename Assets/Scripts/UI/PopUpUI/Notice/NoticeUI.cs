using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
public enum NoticeState
{
    BoxOpen,
    BoxClosed,
    Talk,
}

public class NoticeUI : PopUpUI
{
    private TextMeshProUGUI buttonText;
    private TextMeshProUGUI noticeText;

    public NoticeState state;

    public string button = null;
    public string notice = null;

    private void OnEnable()
    {
        buttonText = GameObject.Find("ExposeButton").transform.GetComponent<TextMeshProUGUI>();
        noticeText = GameObject.Find("NoticeText").transform.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        buttonText.text = button;
        noticeText.text = notice;
    }
    public void SetTexts(string setButton, string setNotice)
    {
        if(setButton != null && setNotice != null)
        {
            button = setButton;
            notice = setNotice;
        }
        else
        {
            button = null;
            notice = null;
        }
    }
}
