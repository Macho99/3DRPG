using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalkState : StateMachineBehaviour
{
    int random;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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
                animator.SetTrigger("Walk_F");
                break;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isWalking", false);
    }
}
