using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using TMPro;
using UnityEngine;
public class InteractionNPC : MonoBehaviour
{
    public string[] sentence;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.Dialogue.InteractionNPC = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Dialogue.InteractionNPC = null;
        }
    }
}
