using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chat Text List", menuName = "NPC/ChatText")]
public class ChatBox : ScriptableObject
{
    public List<string> stringList = new List<string>();
}

public class InteractionNPC : MonoBehaviour
{
    public ChatBox chatDetail;
    public bool isInteraction;
    public bool startTalk;

    private void Awake()
    {
        isInteraction = false;
        startTalk = false;
    }

    private void Start()
    {
        
    }

    private IEnumerator FadeOut(CanvasGroup cg)
    {
        float fadeTime = 1f;
        float accumTime = 0f;
        while (accumTime < fadeTime)
        {
            cg.alpha = Mathf.Lerp(1f, 0f, accumTime / fadeTime);
            yield return 0;
            accumTime += Time.deltaTime;
        }
        cg.alpha = 0f;
        cg.gameObject.SetActive(false);
    }

    public void StartTalk()
    {
        if(gameObject.GetComponent<Baird>() != null)
        {
            gameObject.GetComponent<Baird>().curState = BairdState.Meet;
            startTalk = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && startTalk == true)
        {
            gameObject.GetComponent<Baird>().RotateAgent(other.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            if(gameObject.GetComponent <Baird>() != null) 
            {
                gameObject.GetComponent<Baird>().curState = BairdState.Walk;
                startTalk = false;
            }
        }
    }
}
