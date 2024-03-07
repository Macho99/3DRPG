using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CheckDist : StateMachineBehaviour
{
    Transform target;
    Transform myTf;
    NavMeshAgent agent;
    int random;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        target = animator.GetComponent<DeathKnight>().target;
        myTf = animator.GetComponent<Transform>();
        agent = animator.GetComponent<NavMeshAgent>();

        random = Random.Range(0, 5);
        
        if (random == 0)
        {
            animator.SetBool("WalkAgain", true);
        }

        if (random == 0 && Vector3.Distance(animator.transform.position, target.position) <= agent.stoppingDistance)
        {
            animator.ResetTrigger("Walk_F");
            animator.SetTrigger("Walk_L");
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (target == null)
        {
            animator.SetBool("isAttacking", false);
            return;
        }

        if (Vector3.Distance(myTf.position, target.position) > agent.stoppingDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("WalkAgain", false);
    }
}
