using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkBackState : StateMachineBehaviour
{
    Rigidbody rb;
    NavMeshAgent agent;
    Transform target;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody>();
        agent = animator.GetComponent<NavMeshAgent>();
        target = animator.GetComponent<DeathKnight>().target;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.Move(animator.transform.forward * -2f * Time.deltaTime);
        animator.transform.LookAt(target);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = Vector3.zero;
    }
}
