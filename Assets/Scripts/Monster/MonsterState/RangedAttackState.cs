using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedAttackState : StateMachineBehaviour
{
    Transform target;
    Monster monster;
    RangedMonster rangedMonster;
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster = animator.GetComponent<Monster>();
        rangedMonster = animator.GetComponent<RangedMonster>();
        target = animator.GetComponent<Monster>().target;
        agent = animator.GetComponent<NavMeshAgent>();

        animator.transform.LookAt(target.position);
        monster.state = State.IDLE;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (target == null)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isInRanged", false);
            return;
        }

        if (Vector3.Distance(animator.transform.position, target.position) > agent.stoppingDistance)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isInRanged", false);
        }

        if(rangedMonster.meleeDistance >= Vector3.Distance(animator.transform.position, target.position))
        {
            animator.SetBool("isInMelee", true);
        }

        animator.SetBool("AttackDelay", true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
