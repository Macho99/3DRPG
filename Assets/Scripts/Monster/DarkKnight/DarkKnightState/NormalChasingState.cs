using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NormalChasingState : StateMachineBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform target;
    DeathKnight knight;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        knight = animator.GetComponent<DeathKnight>();
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = animator.GetComponent<DeathKnight>().moveSpeed;
        target = animator.GetComponent<DeathKnight>().target;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (target == null) { return; }

        if (Vector3.Distance(animator.transform.position, target.position) <= agent.stoppingDistance)
        {
            animator.SetBool("isAttacking", true);
        }
        animator.transform.LookAt(target.position);
        agent.SetDestination(target.position);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }
}
