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
    float       hAxis;
    bool        jumpPressed;
    bool        jumpClicked;
    bool        onGround;
    HP          hpComponent;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        hpComponent = GetComponent<HP>();
    }

    void OnEnable()
    {
        hpComponent.onDead += OnDead;
    }

    void OnDisable()
    {
        hpComponent.onDead -= OnDead;
    }

    void Start()
    {
        jumpsAvailable = maxJumpCount;
    }

    private void FixedUpdate()
    {
        // 
        Vector2 currentVelocity = rb.velocity;

        currentVelocity.x = currentVelocity.x * (1 - drag);

        currentVelocity.x = currentVelocity.x + hAxis * acceleration * Time.fixedDeltaTime;

        currentVelocity.x = Mathf.Clamp(currentVelocity.x, -maxSpeed, maxSpeed);

        Collider2D groundCollision = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);

        onGround = groundCollision != null;
        if ((onGround) && (currentVelocity.y <= 0))
        {
            jumpsAvailable = maxJumpCount;
        }

        groundCollider.enabled = onGround;
        airCollider.enabled = !onGround;

        // Salto
        if ((jumpClicked) && (jumpsAvailable > 0))
        {
            currentVelocity.y = jumpSpeed;
            rb.gravityScale = 0.0f;

            jumpTime = Time.fixedTime;

            jumpsAvailable--;

            //UnityEditor.EditorApplication.isPaused = true;
        }
        else if ((jumpPressed) && ((Time.fixedTime - jumpTime) < jumpMaxTime))
        {

        }
        else
        {
            rb.gravityScale = 5.0f;
        }

        // Set da velocidade
        rb.velocity = currentVelocity;

        // Reset jump click
        jumpClicked = false;
    }

    void Update()
    {
        if (hpComponent.hp <= 0) return;
        
        // Movimento em X
        hAxis = Input.GetAxis("Horizontal");
        // Salto
        if (Input.GetButtonDown("Jump"))
        {
            jumpClicked = true;
        }
        jumpPressed = Input.GetButton("Jump");

        // Animação
        Vector2 currentVelocity = rb.velocity;
        anim.SetFloat("AbsVelX", Mathf.Abs(currentVelocity.x));
        anim.SetFloat("VelY", currentVelocity.y);
        anim.SetBool("onGround", onGround);

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

    void OnDead()
    {
        anim.SetTrigger("onDead");

        rb.velocity = new Vector2(0.0f, 200.0f);
    }

    void OnDestroySelf()
    {
        Destroy(gameObject);
    }
}
