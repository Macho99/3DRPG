using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleChestState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<MimicOpen>().StartCoroutine();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform target = animator.GetComponent<MimicOpen>().target;

        if (Input.GetKeyDown(KeyCode.F) && target != null)
        {
            Vector3 knockbackDir = (target.position - animator.transform.position).normalized;
            animator.SetTrigger("Trapped");
            target.GetComponent<Player>().TakeDamage(20, false, 2, knockbackDir);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<MimicOpen>().StopCoroutine();

        
    }
}
