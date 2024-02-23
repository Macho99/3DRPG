using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class BlockState : StateMachineBehaviour
{
    float rotationValue;
    float timer;
    float attackDelay;
    Transform target;
    Transform myTf;
    NavMeshAgent agent;
    Monster monster;
    MonsterShield monsterShield;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster = animator.GetComponent<Monster>();
        monsterShield = animator.GetComponent<MonsterShield>();
        attackDelay = animator.GetComponent<Monster>().attackDelay;
        rotationValue = animator.GetComponent<Monster>().rotationSpeed;
        target = animator.GetComponent<Monster>().target;
        myTf = animator.GetComponent<Transform>();
        agent = animator.GetComponent<NavMeshAgent>();
        timer = monsterShield.guardHit ? monster.attackDelay - 1 : 0f;

        monster.state = State.BLOCK;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if (target == null)
        {
            animator.SetBool("isAttacking", false);
            return;
        }

        Turn(target, myTf);

        if (timer > attackDelay)
        {
            if (Vector3.Distance(animator.transform.position, target.position) > agent.stoppingDistance)
            {
                animator.SetBool("isAttacking", false);
            }
            animator.SetBool("AttackDelay", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monsterShield.guardHit = false;
    }

    private void Turn(Transform target, Transform myTf)
    {
        Vector3 directionToTarget = target.position - myTf.position;
        directionToTarget.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);
        myTf.rotation = Quaternion.Slerp(myTf.rotation, targetRotation, rotationValue * Time.deltaTime);
    }
}
