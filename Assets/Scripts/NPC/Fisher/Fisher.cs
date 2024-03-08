using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum FisherState
{
    Idle,
    Cast
}

public class Fisher : MonoBehaviour
{
    public Animator animator;
    private FisherState currentState;

    private HeadAiming headAiming;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        headAiming = GetComponent<HeadAiming>();
    }

    void Start()
    {
        currentState = FisherState.Idle;

        StartCoroutine(StateTransition());
    }

    private IEnumerator StateTransition()
    {
        while (true)
        {
            switch (currentState)
            {
                case FisherState.Idle:
                    animator.SetBool("IsIdle", true);
                    animator.SetBool("IsCast", false);

                    while (headAiming.mIsLookingTarget)
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(4f);
                    currentState = FisherState.Cast;
                    break;
                case FisherState.Cast:
                    animator.SetBool("IsIdle", false);
                    animator.SetBool("IsCast", true);

                    yield return new WaitForSeconds(8.767f);
                    currentState = FisherState.Idle;
                    break;
            }
            yield return null;
        }
    }
}