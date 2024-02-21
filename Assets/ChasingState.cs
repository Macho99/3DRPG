using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

public class ChasingState : StateMachineBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform target;
    Monster monster;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster = animator.GetComponent<Monster>();
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = animator.GetComponent<Monster>().moveSpeed;
        target = animator.GetComponent<Monster>().target;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (monster.state == State.DEAD)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isChasing", false);
            animator.SetTrigger("Dead");
            return;
        }

        if (target == null && Vector3.Distance(animator.transform.position, monster.spawnPosition) <= agent.stoppingDistance + .5f)
        {
            animator.SetBool("isChasing", false);
            monster.isReturning = false;
            monster.viewAngle = monster.originViewAngle;
            return;
        }

        if (target == null) { return; }

        if (Vector3.Distance(animator.transform.position, monster.spawnPosition) > monster.distanceFromOriginPos)
        {
            agent.SetDestination(monster.spawnPosition);
            monster.target = null;
            target = null;
            monster.isReturning = true;
            return;
        }

        if (Vector3.Distance(animator.transform.position, target.position) <= agent.stoppingDistance)
        {
            animator.SetBool("isAttacking", true);
        }

        agent.SetDestination(target.position);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
