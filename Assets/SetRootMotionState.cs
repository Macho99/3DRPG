using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRootMotionState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.applyRootMotion = !animator.applyRootMotion;
        animator.SetTrigger("isReady");
    }
}
