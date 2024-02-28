using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TwoHandedMotionState : StateMachineBehaviour
{
    float rotationValue;
    Transform target;
    Transform myTf;
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        rotationValue = animator.GetComponent<DeathKnight>().rotationSpeed;
        target = animator.GetComponent<DeathKnight>().target;
        myTf = animator.GetComponent<Transform>();
        animator.applyRootMotion = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (target == null)
        //{
        //    return;
        //}

        //Turn(target, myTf);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("AttackDelay", true);
        animator.applyRootMotion = false;
        agent.SetDestination(myTf.position);
    }
}
