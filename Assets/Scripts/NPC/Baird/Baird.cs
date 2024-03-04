using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

// https://www.youtube.com/watch?v=tKagnP91T6c 참고용 자료 1번
// https://www.youtube.com/watch?v=WBaQ7cRiMjM 2번

public enum BairdState
{
    Idle,
    Walk,
    Meet
}

enum TargetState
{
    First,
    Second,
    Third
}

public class Baird : MonoBehaviour
{
    public Animator animator;
    public BairdState curState;
    private TargetState targetState;

    private Vector3 targetPos;
    public Vector3 firstPos;
    public Vector3 secondPos;
    public Vector3 thirdPos;

    NavMeshAgent theAgent;
    private HeadAiming headAiming;

    InteractionNPC interaction;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        theAgent = GetComponent<NavMeshAgent>();
        headAiming = GetComponent<HeadAiming>();
    }

    private void Start()
    {
        curState = BairdState.Walk;
        targetState = TargetState.First;

        StartCoroutine(BairdCoroutine());
    }

    private IEnumerator BairdCoroutine()
    {
        while(true)
        {
            while (headAiming.mIsLookingTarget)
            {
                yield return null;
            }

            switch (curState)
            {
                case BairdState.Idle:
                    animator.SetBool("IsWalk", false);
                    theAgent.isStopped = true;
                    yield return new WaitForSeconds(4f);
                    curState = BairdState.Walk;
                    break;
                case BairdState.Walk:
                    RotateAgent(targetPos);
                    animator.SetBool("IsWalk", true);

                    theAgent.SetDestination(targetPos);

                    if(Vector3.Distance(transform.position, targetPos) < 0.5)
                    {
                        curState = BairdState.Idle;
                        
                        if(targetState == TargetState.First)
                            targetState = TargetState.Second;
                        else if(targetState == TargetState.Second)
                            targetState = TargetState.Third;
                        else if (targetState == TargetState.Third)
                            targetState = TargetState.First;
                    }
                    break;
                case BairdState.Meet:
                    animator.SetBool("IsWalk", false);
                    theAgent.isStopped = true;
                    break;
            }
            yield return null;
        }
    }

    private void Update()
    {
        switch (targetState)
        {
            case TargetState.First:
                targetPos = firstPos;
                break;
            case TargetState.Second:
                targetPos = secondPos;
                break;
            case TargetState.Third:
                targetPos = thirdPos;
                break;
        }
    }


    public void RotateAgent(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
