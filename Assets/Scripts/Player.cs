using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool  enableInput = true;
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

    public DamageZone   jumpDamageZone;

    public AudioClip    hitSound;
    public AudioClip    deathSound;
    public AudioClip    jumpSound;
    public AudioClip    footstepSound;

    public ParticleSystem   dustPS;
    public ParticleSystem   fallPS;

    Rigidbody2D     rb;
    Animator        anim;
    SpriteRenderer  spriteRenderer;
    float           jumpTime;
    int             jumpsAvailable;
    float           hAxis;
    bool            jumpPressed;
    bool            jumpClicked;
    bool            onGround;
    HP              hpComponent;
    int             score = 0;
    Vector2         previousVelocity;

    float           invulnerabilityFXTimer = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hpComponent = GetComponent<HP>();

        enableInput = true;
    }

    void OnEnable()
    {
        hpComponent.onDead += OnDead;
        hpComponent.onHit += OnHit;

        if (jumpDamageZone) jumpDamageZone.onDamageDealt += OnJumpDamageReaction;
    }

    void OnDisable()
    {
        hpComponent.onDead -= OnDead;
        hpComponent.onHit -= OnHit;

        if (jumpDamageZone) jumpDamageZone.onDamageDealt -= OnJumpDamageReaction;
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

        if (jumpDamageZone)
        {
            jumpDamageZone.enabled = (currentVelocity.y < 0.0f);
        }

        if (onGround)
        {
            if (previousVelocity.y < -0.2f)
            {
                fallPS.Play();
            }
        }

        previousVelocity = currentVelocity;
    }

    void Update()
    {
        if (hpComponent.hp <= 0) return;

        if (enableInput)
        {
            // Movimento em X
            hAxis = Input.GetAxis("Horizontal");
            // Salto
            if (Input.GetButtonDown("Jump"))
            {
                jumpClicked = true;

                if (onGround)
                {
                    if (jumpSound)
                    {
                        SoundMng.instance.PlaySound(jumpSound, 0.5f);
                    }
                }
            }
            jumpPressed = Input.GetButton("Jump");
        }

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

        if (hpComponent.isInvulnerable)
        {
            invulnerabilityFXTimer = invulnerabilityFXTimer - Time.deltaTime;
            if (invulnerabilityFXTimer < 0)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                invulnerabilityFXTimer = 0.1f;
            }
        }
        else
        {
            spriteRenderer.enabled = true;
            invulnerabilityFXTimer = 0.0f;
        }

        if (dustPS)
        {
            var emission = dustPS.emission;
            emission.enabled = onGround;
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

        rb.velocity = new Vector2(0.0f, 300.0f);

        if (deathSound)
        {
            SoundMng.instance.PlaySound(deathSound, 0.5f);
        }
    }

    void OnHit()
    {
        anim.SetTrigger("onHit");

        rb.velocity = new Vector2(0.0f, 200.0f);

        if (hitSound)
        {
            SoundMng.instance.PlaySound(hitSound, 0.5f);
        }
    }

    void OnJumpDamageReaction(HP target, float damage)
    {
        rb.velocity = new Vector2(0.0f, 200.0f);
    }

    void OnDestroySelf()
    {
        Destroy(gameObject);
    }

    public void AddScore(int s)
    {
        score += s;

        if (GameMng.instance.highScore < score)
        {
            GameMng.instance.highScore = score;
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void PlayFootstep()
    {
        if (Mathf.Abs(previousVelocity.x) >= 5.0f)
        {
            if (footstepSound)
            {
                SoundMng.instance.PlaySound(footstepSound, Random.Range(0.1f, 0.15f), Random.Range(0.75f, 1.25f));
            }
        }
    }

    public void ResetMovement()
    {
        hAxis = 0.0f;
        jumpPressed = false;
        jumpClicked = false;
    }

    public void SetHAxis(float h)
    {
        hAxis = h;
    }

    public void SetJump(bool b)
    {
        jumpClicked = b;
    }
}
