using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetModeState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<DeathKnight>().bossState == BossState.TWOHANDED)
        {
            animator.SetBool("Normal", false);
            animator.SetBool("TwoHanded", true);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
