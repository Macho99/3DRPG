using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttackState : StateMachineBehaviour
{
    Transform target;
    Monster monster;
    RangedMonster rangedMonster;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster = animator.GetComponent<Monster>();
        rangedMonster = animator.GetComponent<RangedMonster>();
        target = animator.GetComponent<Monster>().target;
        animator.transform.LookAt(target.position);
        monster.state = State.IDLE;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (target == null)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isInMelee", false);
            return;
        }

        if (Vector3.Distance(animator.transform.position, target.position) > rangedMonster.meleeDistance)
        {
            animator.SetBool("isInMelee", false);
        }

        animator.SetBool("AttackDelay", true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
