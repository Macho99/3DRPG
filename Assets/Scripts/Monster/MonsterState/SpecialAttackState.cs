using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpecialAttackState : StateMachineBehaviour
{
    Transform target;
    Monster monster;
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster = animator.GetComponent<Monster>();
        target = animator.GetComponent<Monster>().target;
        agent = animator.GetComponent<NavMeshAgent>();

        animator.transform.LookAt(target.position);
        monster.state = State.IDLE;

        monster.attackCol.enabled = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (target == null)
        {
            animator.SetBool("isAttacking", false);
            return;
        }

        if (Vector3.Distance(animator.transform.position, target.position) > agent.stoppingDistance)
        {
            animator.SetBool("isAttacking", false);
        }

        animator.SetBool("AttackDelay", true);
        animator.SetBool("SpecialAttack", false);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster.attackCol.enabled = false;
    }
}
