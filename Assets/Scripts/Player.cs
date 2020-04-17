using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed = 200.0f;
    public float acceleration = 50.0f;
    public float drag = 0.5f;
    public float jumpSpeed = 300.0f;
    public float jumpMaxTime = 0.1f;

    public Collider2D groundCollider;
    public Collider2D airCollider;

    public int   maxJumpCount = 2;

    public Transform groundCheck;
    public float     groundCheckRadius = 1;
    public LayerMask groundLayers;

    Rigidbody2D rb;
    Animator    anim;
    float       jumpTime;
    int         jumpsAvailable;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        jumpsAvailable = maxJumpCount;
    }

    void Update()
    {
        // Movimento em X
        float hAxis = Input.GetAxis("Horizontal");

        Vector2 currentVelocity = rb.velocity;

        currentVelocity.x = currentVelocity.x * (1 - drag);

        currentVelocity.x = currentVelocity.x + hAxis * acceleration * Time.deltaTime;

        currentVelocity.x = Mathf.Clamp(currentVelocity.x, -maxSpeed, maxSpeed);

        Collider2D groundCollision = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);

        bool onGround = groundCollision != null;
        if ((onGround) && (currentVelocity.y <= 0))
        {
            jumpsAvailable = maxJumpCount;
        }

        groundCollider.enabled = onGround;
        airCollider.enabled = !onGround;

        // Salto
        if ((Input.GetButtonDown("Jump")) && (jumpsAvailable > 0))
        {
            currentVelocity.y = jumpSpeed;
            rb.gravityScale = 0.0f;

            jumpTime = Time.time;

            jumpsAvailable--;

            //UnityEditor.EditorApplication.isPaused = true;
        }
        else if ((Input.GetButton("Jump")) && ((Time.time - jumpTime) < jumpMaxTime))
        {
                
        }
        else
        {
            rb.gravityScale = 5.0f;
        }

        // Set da velocidade
        rb.velocity = currentVelocity;

        // Animação
        anim.SetFloat("AbsVelX", Mathf.Abs(currentVelocity.x));

        if (currentVelocity.x < -0.5f)
        {
            if (transform.right.x > 0)
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (currentVelocity.x > 0.5f)
        {
            if (transform.right.x < 0)
                transform.rotation = Quaternion.identity;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }
}
