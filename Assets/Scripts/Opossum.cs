using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : MonoBehaviour
{
    [SerializeField] float     moveSpeed = 150.0f;
    [SerializeField] Transform groundDetector = null;
    [SerializeField] Transform wallDetector = null;
    [SerializeField] float     detectionRadius = 3.0f;
    [SerializeField] LayerMask groundLayers;

    Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(moveSpeed, 0.0f);
    }

    void FixedUpdate()
    {
        Vector2 currentVelocity = rigidBody.velocity;

        bool alreadyTurned = false;

        if ((groundDetector) && (Mathf.Abs(currentVelocity.y) < 0.1f))
        {
            Collider2D groundCollision = Physics2D.OverlapCircle(groundDetector.position, detectionRadius, groundLayers);

            bool onGround = groundCollision != null;

            if (!onGround)
            {
                TurnBack();
                alreadyTurned = true;
            }
        }

        if ((wallDetector) && (Mathf.Abs(currentVelocity.y) < 0.1f) && (!alreadyTurned))
        {
            Collider2D wallCollision = Physics2D.OverlapCircle(wallDetector.position, detectionRadius, groundLayers);

            bool onWall = wallCollision != null;

            if (onWall)
            {
                TurnBack();
            }
        }

        currentVelocity.x = transform.right.x * moveSpeed;

        rigidBody.velocity = currentVelocity;
    }

    void TurnBack()
    {
        transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
    }

    private void OnDrawGizmos()
    {
        if (groundDetector != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(groundDetector.position, detectionRadius);
        }
        if (wallDetector != null)
        {
            Gizmos.color = new Color(1.0f, 0.5f, 0.0f, 1.0f);
            Gizmos.DrawSphere(wallDetector.position, detectionRadius);
        }
    }
}
