using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcAttackState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("SpecialAttack"))
        {
            animator.GetComponent<Monster>().SetAttackTypeInfo(10, true, .2f, Vector3.zero);
        }
        else
        {
            animator.GetComponent<Monster>().SetAttackTypeInfo(15, true, 0, Vector3.zero);
        }
    }
}
