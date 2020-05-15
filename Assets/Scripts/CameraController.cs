using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField] Vector3   offset;
    [SerializeField] float     feedbackLoopFactor = 0.1f;
    [SerializeField] Rect      cameraTrap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followTarget == null)
        {
            return;
        }

        Vector3 targetPosition = followTarget.position + offset;
        targetPosition.z = transform.position.z;

        Rect rect = GetWorldRect();

        if (targetPosition.x < rect.xMin) rect.xMin = targetPosition.x;
        else if (targetPosition.x > rect.xMax) rect.xMax = targetPosition.x;

        if (targetPosition.y < rect.yMin) rect.yMin = targetPosition.y;
        else if (targetPosition.y > rect.yMax) rect.yMax = targetPosition.y;

        Vector3 movePosition = rect.center;
        movePosition.z = transform.position.z;

        Vector3 delta = movePosition - transform.position;

        transform.position = transform.position + delta * feedbackLoopFactor;
    }

    Rect GetWorldRect()
    {
        Rect rect = cameraTrap;
        rect.position += new Vector2(-rect.width * 0.5f, -rect.height * 0.5f);
        rect.position += new Vector2(transform.position.x, transform.position.y);

        return rect;
    }

    private void OnDrawGizmosSelected()
    {
        Rect newRect = GetWorldRect();

        Gizmos.color = new Color(0, 0.5f, 0.0f, 1.0f);

        Vector3 p1 = new Vector3(newRect.xMin, newRect.yMin, 0);
        Vector3 p2 = new Vector3(newRect.xMax, newRect.yMin, 0);
        Vector3 p3 = new Vector3(newRect.xMax, newRect.yMax, 0);
        Vector3 p4 = new Vector3(newRect.xMin, newRect.yMax, 0);

        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
    }

    public void SetFollowTarget(Transform p)
    {
        followTarget = p;
    }
}
