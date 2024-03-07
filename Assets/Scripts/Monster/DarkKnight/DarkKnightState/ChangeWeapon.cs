using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Normal", false);
        animator.SetBool("TwoHanded", true);

        animator.GetComponent<DeathKnight>().ChangeWeapon();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("MeleeCombo1");
        animator.ResetTrigger("MeleeCombo2");
        animator.ResetTrigger("MeleeCombo3");
        animator.ResetTrigger("Skill1");
        animator.ResetTrigger("Skill2");
        animator.ResetTrigger("Skill3");
        animator.ResetTrigger("Skill4");
    }
}
