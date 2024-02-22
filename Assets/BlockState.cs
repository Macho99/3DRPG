using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlockState : StateMachineBehaviour
{
    float timer;
    float attackDelay;
    Transform target;
    NavMeshAgent agent;
    Monster monster;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackDelay = animator.GetComponent<Monster>().attackDelay;
        timer = 0f;
        target = animator.GetComponent<Monster>().target;
        agent = animator.GetComponent<NavMeshAgent>();
        monster = animator.GetComponent<Monster>();

        monster.state = State.BLOCK;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if (target == null)
        {
            animator.SetBool("isAttacking", false);
            return;
        }

        animator.transform.LookAt(target.position);

        if (timer > attackDelay)
        {
            if (Vector3.Distance(animator.transform.position, target.position) > agent.stoppingDistance)
            {
                animator.SetBool("isAttacking", false);
            }
            animator.SetBool("AttackDelay", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
