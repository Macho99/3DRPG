using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePatrolState : StateMachineBehaviour
{
    private Transform target;
    float timer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        target = animator.GetComponent<Monster>().target;
        timer = 0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        target = animator.gameObject.GetComponent<Monster>().target;

        if (target != null)
        {
            animator.SetBool("isChasing", true);
        }

        if (timer > 3f)
        {
            animator.SetBool("isPatrolling", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
