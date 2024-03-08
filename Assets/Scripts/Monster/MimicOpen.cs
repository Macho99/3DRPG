using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicOpen : MonoBehaviour
{
    private float viewRadius;
    private int targetMask;
    private int viewAngle;
    public Transform target;

    private void Start()
    {
        targetMask = LayerMask.GetMask("Player");
        viewAngle = 60;
        viewRadius = 2;
    }
    protected IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, viewRadius);

    //    if (target != null)
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawLine(transform.position, target.position);
    //    }

    //    Gizmos.color = Color.red;

    //    int segments = 36;
    //    float step = viewAngle / segments;
    //    for (int i = 0; i < segments; i++)
    //    {
    //        float angle = i * step - viewAngle / 2;
    //        float x = Mathf.Sin(Mathf.Deg2Rad * angle) * viewRadius;
    //        float z = Mathf.Cos(Mathf.Deg2Rad * angle) * viewRadius;
    //        Vector3 start = transform.position + Vector3.up * 1f;
    //        Vector3 dir = transform.TransformDirection(new Vector3(x, 0, z));
    //        Gizmos.DrawLine(start, transform.position + dir);
    //    }
    //}

    private void FindVisibleTargets()
    {
        target = null;

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (Physics.Raycast(transform.position + Vector3.up * 1f, dirToTarget, distToTarget, targetMask))
                {
                    //this.target = target;
                    if (target.TryGetComponent(out Player player))
                    {
                        this.target = target;
                    }
                }
            }
        }
    }

    public void StopCoroutine()
    {
        StopAllCoroutines();
    }

    public void StartCoroutine()
    {
        _ = StartCoroutine(FindTargetWithDelay(.2f));
    }
}
