using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Baird : MonoBehaviour
{
    public Animator animator;

    public GameObject targetPos;

    NavMeshAgent theAgent;
    private HeadAiming headAiming;

    bool isHeal;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        theAgent = GetComponent<NavMeshAgent>();
        headAiming = GetComponent<HeadAiming>();
        isHeal = false;
    }

    private void Start()
    {
        NextTargeting();
    }

    private void Update()
    {
        if (theAgent.remainingDistance < 0.2f)
        {
            theAgent.isStopped = true;
            animator.SetBool("IsWalk", false);
            Invoke("NextTargeting", 4.5f);
        }
    }

    void NextTargeting()
    {
        theAgent.isStopped = false;
        theAgent.SetDestination(targetPos.transform.position);
        animator.SetBool("IsWalk", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isHeal = true;
            StartCoroutine(BairdHealing());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            theAgent.isStopped = true;
            animator.SetBool("IsWalk", false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            NextTargeting();
            isHeal = false;
        }
    }

    IEnumerator BairdHealing()
    {
        while (isHeal)
        {
            GameManager.Stat.AddCurHP(5);
            GameManager.Stat.AddCurMP(5);
            yield return new WaitForSeconds(2f);
        }
    }

}
