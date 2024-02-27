using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NormalAttackCDState : StateMachineBehaviour
{
    float rotationValue;
    float timer;
    float attackDelay;
    Transform target;
    Transform myTf;
    NavMeshAgent agent;
    DeathKnight knight;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        knight = animator.GetComponent<DeathKnight>();
        attackDelay = animator.GetComponent<DeathKnight>().attackDelay;
        rotationValue = animator.GetComponent<DeathKnight>().rotationSpeed;
        target = animator.GetComponent<DeathKnight>().target;
        myTf = animator.GetComponent<Transform>();
        agent = animator.GetComponent<NavMeshAgent>();
        timer = 0f;

        ChooseNextMotion(animator);
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

        // 공격 준비가 끝나면
        if (timer > attackDelay)
        {
            // 공격 사거리 안에 있지 않다면
            if (Vector3.Distance(myTf.position, target.position) > agent.stoppingDistance)
            {
                animator.SetBool("isAttacking", false);
            }
            animator.SetBool("AttackDelay", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    private void Turn(Transform target, Transform myTf)
    {
        Vector3 directionToTarget = target.position - myTf.position;
        directionToTarget.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);
        myTf.rotation = Quaternion.Slerp(myTf.rotation, targetRotation, rotationValue * Time.deltaTime);
    }

    void ChooseNextMotion(Animator animator)
    {
        int rand = Random.Range(0, 7);

        switch (rand)
        {
            case 0:
                animator.SetTrigger("MeleeAttack1");
                agent.stoppingDistance = knight.meleeAttackRange;
                break;
            case 1:
                animator.SetTrigger("MeleeAttack2");
                agent.stoppingDistance = knight.meleeAttackRange;
                break;
            case 2:
                animator.SetTrigger("MeleeCombo1");
                agent.stoppingDistance = knight.meleeAttackRange;
                break;
            case 3:
                animator.SetTrigger("MeleeCombo2");
                agent.stoppingDistance = knight.meleeAttackRange;
                break;
            case 4:
                animator.SetTrigger("Skill1");
                agent.stoppingDistance = knight.skillAttackRange;
                break;
            case 5:
                animator.SetTrigger("Skill2");
                agent.stoppingDistance = knight.skillAttackRange;
                break;
            case 6:
                animator.SetTrigger("Skill3");
                agent.stoppingDistance = knight.skillAttackRange;
                break;
            default:
                animator.SetTrigger("MeleeAttack1");
                agent.stoppingDistance = knight.meleeAttackRange;
                break;
        }
    }
}
