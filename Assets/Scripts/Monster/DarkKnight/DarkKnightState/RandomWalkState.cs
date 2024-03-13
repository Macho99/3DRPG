using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomWalkState : StateMachineBehaviour
{
    int random;
    Transform target;
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        target = animator.GetComponent<DeathKnight>().target;
        agent = animator.GetComponent<NavMeshAgent>();

        random = Random.Range(0, 4);
        switch (random)
        {
            case 0:
                animator.SetTrigger("Walk_F");
                break;
            case 1:
                animator.SetTrigger("Walk_B");
                break;
            case 2:
                animator.SetTrigger("Walk_L");
                break;
            case 3:
                animator.SetTrigger("Walk_R");
                break;
            default:
                animator.SetTrigger("Walk_L");
                break;
        }

        if (random == 0 && Vector3.Distance(animator.transform.position, target.position) <= agent.stoppingDistance)
        {
            animator.ResetTrigger("Walk_F");
            animator.SetTrigger("Walk_L");
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.LookAt(new Vector3(target.position.x, animator.transform.position.y, target.position.z));
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isWalking", false);
    }
}
