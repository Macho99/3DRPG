using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<DeathKnight>().ChangeWeapon();
    }
}
