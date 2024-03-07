using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using TMPro;
using UnityEngine;
public class InteractionNPC : MonoBehaviour
{
    public string[] sentence;
    public Quaternion savePos;

    private void Start()
    {
        Vector3 direction = (transform.forward - transform.position).normalized;
        savePos = Quaternion.LookRotation(direction);
    }


    public void RotateAgent(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.Dialogue.InteractionNPC = this;
            RotateAgent(other.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Dialogue.InteractionNPC = null;
            transform.rotation = Quaternion.Slerp(transform.rotation, savePos, Time.deltaTime * 5f);
        }
    }
}
