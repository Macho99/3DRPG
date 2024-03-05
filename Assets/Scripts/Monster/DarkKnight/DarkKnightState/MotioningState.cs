using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MotioningState : StateMachineBehaviour
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
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (target == null)
        {
            return;
        }

        Turn(target, myTf);
        agent.SetDestination(myTf.position);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("AttackDelay", true);
    }

    private void Turn(Transform target, Transform myTf)
    {
        Vector3 directionToTarget = target.position - myTf.position;
        directionToTarget.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);
        myTf.rotation = Quaternion.Slerp(myTf.rotation, targetRotation, rotationValue * Time.deltaTime);
    }
}
