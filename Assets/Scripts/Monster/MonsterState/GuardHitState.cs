using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardHitState : StateMachineBehaviour
{
    MonsterShield monsterShield;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monsterShield = animator.GetComponent<MonsterShield>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monsterShield.guardHit = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Guard");
    }
}
