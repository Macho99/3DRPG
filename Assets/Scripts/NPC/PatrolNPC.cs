using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolNPC : MonoBehaviour
{
    public Vector3[] patrolPoints;
    private Queue<Vector3> patrolQueue;
    private Vector3 curPatrolPoint;

    Animator animator;
    NavMeshAgent theAgent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        theAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        for(int i = 0; i < patrolPoints.Length; i++)
        {
            patrolQueue.Enqueue(patrolPoints[i]);
        }
        curPatrolPoint = patrolQueue.Dequeue();
    }

    private void Update()
    {
        if(patrolQueue.Count > 0)
        {

        }
        else
        {
            ReStartPatrol();
        }
    }

    private void ReStartPatrol()
    {
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolQueue.Enqueue(patrolPoints[i]);
        }
    }
}
