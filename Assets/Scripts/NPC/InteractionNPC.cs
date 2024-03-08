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
        savePos = transform.rotation;
    }


    public void RotateAgent(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.Dialogue.InteractionNPC = this;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Vector3 direction = (other.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Dialogue.InteractionNPC = null;
            if (gameObject.GetComponent<Baird>())
            {
                return;
            }
            else
            {
                StartCoroutine(RotateTowards(savePos));
            }
        }
    }

    IEnumerator RotateTowards(Quaternion targetRotation)
    {
        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
            yield return null;
        }
    }
}
