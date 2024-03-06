using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private NoticeUI notice;

    private void Awake()
    {
        notice = GameManager.Resource.Load<NoticeUI>("UI/WIndowUI/Notice/NoticeUI");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "NPC")
        {
            notice.notice = "대화하기";
            if(notice.notice != null)
            {
                GameManager.UI.ShowWindowUI(notice);
                GameManager.Dialogue.isTalking = false;
            }
        }
    }

    

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "NPC")
        {
            GameManager.UI.ClearWindowUI();
            GameManager.Dialogue.isTalking = false;
        }
    }
}

