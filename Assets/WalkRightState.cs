using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkRightState : StateMachineBehaviour
{
    Rigidbody rb;
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody>();
        agent = animator.GetComponent<NavMeshAgent>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = animator.transform.right * 15f;
        agent.SetDestination(animator.transform.position);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = Vector3.zero;
    }
}
