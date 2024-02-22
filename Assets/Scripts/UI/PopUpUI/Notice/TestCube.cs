using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestCube : MonoBehaviour
{
    public NoticeUI noticeUI;
    public bool isOpen = false;

    private void Awake()
    {
        noticeUI = Resources.Load<NoticeUI>("UI/PopUpUI/Notice/NoticeUI");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //GameManager.UI.ShowPopUpUI(noticeUI);
            _ = StartCoroutine(LateShow());
        }
    }

    private IEnumerator LateShow()
    {
        noticeUI.SetTexts("E", $": {gameObject.name} 상호작용");
        yield return new WaitForSeconds( 0.1f );
        GameManager.UI.ShowPopUpUI(noticeUI);
    }

    private void OnTriggerExit(Collider other)
    {
        GameManager.UI.ClearPopUpUI();
    }
}
