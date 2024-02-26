using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        Turn(target, transform);
    }
    private void Turn(Transform target, Transform myTf)
    {
        Vector3 directionToTarget = target.position - myTf.position;
        directionToTarget.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);
        myTf.rotation = Quaternion.Slerp(myTf.rotation, targetRotation, 5f * Time.deltaTime);
    }
}
