using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedChasingState : StateMachineBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform target;
    Monster monster;
    float timer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster = animator.GetComponent<Monster>();
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = animator.GetComponent<Monster>().moveSpeed;
        target = animator.GetComponent<Monster>().target;

        monster.state = State.IDLE;
        animator.SetBool("isInRanged", false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (target == null && Vector3.Distance(animator.transform.position, monster.spawnPosition) <= .5f)
        {
            animator.transform.forward = monster.spawnDir;
            agent.stoppingDistance = monster.attackRange;
            animator.SetBool("isChasing", false);
            monster.isReturning = false;
            monster.viewAngle = monster.originViewAngle;
            return;
        }

        if (target == null) { return; }

        if (Vector3.Distance(animator.transform.position, monster.spawnPosition) > monster.distanceFromOriginPos)
        {
            agent.stoppingDistance = 0f;
            agent.SetDestination(monster.spawnPosition);
            monster.target = null;
            target = null;
            monster.isReturning = true;
            return;
        }

        if (Vector3.Distance(animator.transform.position, target.position) <= agent.stoppingDistance)
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isInRanged", true);
        }

        agent.SetDestination(target.position);

        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(target.position, path);

        if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
        {
            timer += Time.deltaTime;

            if (timer > 3f)
            {
                agent.stoppingDistance = 0f;
                agent.SetDestination(monster.spawnPosition);
                monster.target = null;
                target = null;
                monster.isReturning = true;
                timer = 0f;
            }
            return;
        }

        timer = 0f;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }
}
