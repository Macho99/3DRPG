using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingAttackState : StateMachineBehaviour
{
    Animator anim;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        anim = animator;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public void OnAttackReady()
    {
        anim.SetFloat("AttackSpeed", .1f);
    }

    public void OnAttackStart()
    {
        anim.SetFloat("AttackSpeed", 1f);
    }

    public void OnAttackEnd()
    {
        anim.SetFloat("AttackSpeed", .5f);
    }
}
