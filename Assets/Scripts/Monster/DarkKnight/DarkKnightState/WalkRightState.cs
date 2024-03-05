using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkRightState : StateMachineBehaviour
{
    Rigidbody rb;
    NavMeshAgent agent;
    Transform target;
    float timer;
    float randomTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody>();
        agent = animator.GetComponent<NavMeshAgent>();
        target = animator.GetComponent<DeathKnight>().target;
        timer = 0f;
        randomTime = Random.Range(1f, 3f);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        agent.Move(animator.transform.right * 2f * Time.deltaTime);
        animator.transform.LookAt(target);

        if (timer > randomTime)
        {
            animator.SetTrigger("isTimeOver");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = Vector3.zero;
        animator.ResetTrigger("isTimeOver");
    }
}
