using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FFarmerState
{
    Idle,
    Pick,
    Look
}

public class FarmerFemale : MonoBehaviour
{
    public Animator animator;
    private FFarmerState currentState;

    private HeadAiming headAiming;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        headAiming = GetComponent<HeadAiming>();
    }

    void Start()
    {
        currentState = FFarmerState.Idle;

        StartCoroutine(StateTransition());
    }


    private IEnumerator StateTransition()
    {
        while (true)
        {
            switch (currentState)
            {
                case FFarmerState.Idle:
                    animator.SetBool("IsIdle", true);
                    animator.SetBool("IsPick", false);

                    while (headAiming.mIsLookingTarget)
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(8f);
                    currentState = FFarmerState.Pick;
                    break;
                case FFarmerState.Pick:
                    animator.SetBool("IsIdle", false);
                    animator.SetBool("IsPick", true);

                    yield return new WaitForSeconds(7f);
                    currentState = FFarmerState.Idle;
                    break;
            }
            yield return null;
        }
    }
}
