using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    // ��ȣ�ۿ� UI ����
    public InteractionNPC nearbyNPC;

    private NoticeUI notice;

    private void Awake()
    {
        notice = GameManager.Resource.Load<NoticeUI>("UI/WIndowUI/Notice/NoticeUI");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "NPC")
        {
            nearbyNPC = other.GetComponent<InteractionNPC>();

            notice.notice = "��ȭ�ϱ�";
            if(notice.notice != null)
            {
                GameManager.UI.ShowWindowUI(notice);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "NPC")
        {
            nearbyNPC = null;

            GameManager.UI.ClearWindowUI();
        }
    }
}

