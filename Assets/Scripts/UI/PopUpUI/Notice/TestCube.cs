using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TestCube : MonoBehaviour
{
    public NoticeUI noticeUI;
    public SOItem testItem;
    public bool haveItem;

    private void Awake()
    {
        noticeUI = Resources.Load<NoticeUI>("UI/PopUpUI/Notice/NoticeUI");
        haveItem = true;
    }

    private void Update()
    {
        if(haveItem == true)
        {
            noticeUI.SetTexts("E", $": {gameObject.name} »Æ¿Œ");
        }
        else if (haveItem == false)
        {
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PlayerUseUI>())
        {
            //GameManager.UI.ShowPopUpUI(noticeUI);
            other.GetComponent<PlayerUseUI>().testCube = this;
            _ = StartCoroutine(ShowNoticeUI());
        }
        else
        {
            return;
        }
    }

    private IEnumerator ShowNoticeUI()
    {
        yield return new WaitForSeconds( 0.1f );
        GameManager.UI.ShowPopUpUI(noticeUI);
    }

    public IEnumerator ShowGainItem()
    {
        var window = GameManager.UI.ShowWindowUI<GainItemWindow>("UI/WIndowUI/GainItemNotice");
        window.itemName.text = testItem.itemName;
        window.itemImage.sprite = testItem.itemIcon;
        yield return null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PlayerUseUI>())
        {
            other.GetComponent<PlayerUseUI>().onInteractionUI = false;
            other.GetComponent<PlayerUseUI>().testCube = null;
            GameManager.UI.ClearPopUpUI();
            GameManager.UI.ClearWindowUI();
        }
    }
}
