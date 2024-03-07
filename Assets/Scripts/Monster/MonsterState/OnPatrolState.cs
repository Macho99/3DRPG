using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OnPatrolState : StateMachineBehaviour
{
    Transform target;
    List<Transform> wayPoints = new List<Transform>();
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();

        foreach (Transform point in animator.GetComponent<Monster>().wayPoints)
        {
            this.wayPoints.Add(point);
        }

        agent.speed = animator.GetComponent<Monster>().walkSpeed;
        agent.SetDestination(SetRandomWayPoint(wayPoints, animator).position);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= .2f)
        {
            animator.SetBool("isPatrolling", false);
        }

        target = animator.gameObject.GetComponent<Monster>().target;

        if (target != null)
        {
            animator.SetBool("isChasing", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isPatrolling", false);
    }

    private Transform SetRandomWayPoint(List<Transform> wayPoints, Animator anim)
    {
        Transform wayPoint = null;

        while (wayPoint == null)
        {
            int randomIndex = Random.Range(0, wayPoints.Count);
            Transform randomWayPoint = wayPoints[randomIndex];

            if (Vector3.Distance(anim.transform.position, randomWayPoint.position) >= 0.5f)
            {
                wayPoint = randomWayPoint;
            }
        }

        return wayPoint;
    }
}
