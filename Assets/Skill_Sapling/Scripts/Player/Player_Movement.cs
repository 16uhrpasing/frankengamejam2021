using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Animator anim;
    

    public enum State
    {
        Idle,
        Walk,
        Jump,
        Dash,
        Dive
    }

    private State currentstate;

    private Rigidbody2D rb;
    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    [SerializeField] private float gravity = 0.01f;
    [SerializeField] private float fallingJumpTimer = 0.15f;
    private float timeSinceGround;

    private bool isGrounded;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private LayerMask ground;

    [Header("Damping")]

    [Range(0f, 1f)]
    [SerializeField] private float basicDamping, stoppingDamping, turningDamping;
    private float acc;
 
    private float jumpBuffer = 0f;

    [Header("Dash")]
    [SerializeField] private float dashForce;
    [SerializeField] private float startDashTimer;
    [SerializeField] private float dashCooldown;
    private float timeUntilDash = 0;

    [HideInInspector] public float currentDashTimer;
    private bool isDashing;
    private float dashDirection;
    [SerializeField] private GameEvent dashEvent;

    [Header("Walljump")]
    [SerializeField] private float wallJumptime = 0.2f;
    [SerializeField] private float wallSlideSpeed = 0.3f;
    [SerializeField] private float wallDistance = 0.5f;
    [SerializeField] private float wallJumpTotalEnergy = 100.0f;
    [SerializeField] private float wallJumpCurrentEnergy = 100.0f;
    [SerializeField] private float wallJumpExhaustion = 30.0f;
    [SerializeField] private float wallJumpRegenPerSec = 15.0f;

    [SerializeField] private LayerMask lava;

    private bool isWallSliding = false;
    private RaycastHit2D wallCheckHit;
    private float jumpTime;

    private bool isFacingRight;
    private float dir;



    private bool swimming = false;
    private bool justSwam = false;
    private AbilityBag bag;

    [SerializeField] private GameEvent respawnEvent;
    private Vector3 spawnPos;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        bag = GetComponent<AbilityBag>();
        spawnPos = transform.position;
    }

    
    public void Respawn()
    {
        rb.velocity = Vector2.zero;
        acc = 0;
        transform.position = spawnPos;
    }
    private void Update()
    {

        if (rb.velocity.y > 0.1f && !isGrounded)
        {
            currentstate = State.Jump;
        }
        else if (rb.velocity.y < -0.1f && !isGrounded)
            
        {
            currentstate = State.Dive;
        }
        else
        {
            if (Mathf.Abs(rb.velocity.x) > 1.5f)
            {
                if(currentDashTimer > 0)
                {
                    currentstate = State.Dash;
                }
                else
                {
                    currentstate = State.Walk;
                } 
            }
            else
                currentstate = State.Idle;
        }

        
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector2(-0.6f, 0.6f);
        }
        else if (rb.velocity.x < 0)
        {
            
            transform.localScale = new Vector2(0.6f, 0.6f);
        }

        anim.SetInteger("state", (int)currentstate);

        if (Input.GetKeyDown("r"))
        {
            respawnEvent.Raise();
            Respawn();
        }
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 0.02f);
        bool swimRn = false;
        foreach(Collider2D coll in colls)
        {
            if(coll.gameObject.layer == 8 || coll.gameObject.layer == 9)
            {
                swimming = true;
                swimRn = true;
                justSwam = true;
            }
        }

        if (!swimRn)
        {
            if (justSwam)
            {
                acc = -1f;
                justSwam = false;
            }
            swimming = false;
        }

        if (!swimming)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpBuffer = 0.15f;
            }
            dir = Input.GetAxisRaw("Horizontal");
            if (dir >= 0)
            {
                isFacingRight = true;
            }
            else
            {
                isFacingRight = false;
            }

            #region dash
            if(bag.currentAbility == Ability.Dash)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && rb.velocity.x != 0 && timeUntilDash <= 0)
                {
                    timeUntilDash = dashCooldown;
                    isDashing = true;
                    currentDashTimer = startDashTimer;
                    if (isFacingRight)
                        dashDirection = 1;
                    else
                        dashDirection = -1;
                    rb.velocity = Vector2.zero;
                }

                if (timeUntilDash > 0)
                {
                    timeUntilDash -= Time.deltaTime;
                    VariableRaise.publicCooldown = dashCooldown;
                    VariableRaise.RaiseFloat(dashEvent, timeUntilDash);
                }

                if (isDashing)
                {
                    rb.velocity = transform.right * dashDirection * dashForce;

                    currentDashTimer -= Time.deltaTime;

                    if (currentDashTimer <= 0)
                    {
                        isDashing = false;
                    }
                }
            }

            #endregion
            #region walljump
            if (bag.currentAbility == Ability.WallJump)
            {
                if (isWallSliding && Input.GetKeyDown(KeyCode.Space))
                {
                    VariableRaise.publicCooldown = wallJumpTotalEnergy;
                    //nicht genug energie zum jumpen

                    if (rb.IsTouchingLayers(lava))
                    {
                        wallJumpCurrentEnergy = 0;
                    }

                    if ((wallJumpCurrentEnergy - wallJumpExhaustion) <= 0)
                    {
                        Debug.Log("not enough energy to jump: " + wallJumpCurrentEnergy);
                        return;
                    }

                    wallJumpCurrentEnergy -= wallJumpExhaustion;
                    Debug.Log("new energy: " + wallJumpCurrentEnergy);
                    acc = -0.2f;
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1.5f);
                }

                //Regenerate wallJump
                if (wallJumpCurrentEnergy < wallJumpTotalEnergy)
                {
                    wallJumpCurrentEnergy += Time.deltaTime * wallJumpRegenPerSec;
                    if (wallJumpCurrentEnergy > wallJumpTotalEnergy) wallJumpCurrentEnergy = wallJumpTotalEnergy;
                    //Debug.Log("regen: " + wallJumpCurrentEnergy);
                    VariableRaise.RaiseFloat(dashEvent, 100.0f - wallJumpCurrentEnergy);
                }
            }
            #endregion
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = Vector2.up * jumpForce * 2.5f;
            }
        }
       
    }

    private void FixedUpdate()
    {
        if (!swimming)
        {
            #region horizontal
            float horizontalVelocity = rb.velocity.x;
            horizontalVelocity += Input.GetAxisRaw("Horizontal");

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.01f)
                horizontalVelocity *= Mathf.Pow(1 - stoppingDamping, Time.deltaTime * 10f);
            else if (Mathf.Sign(Input.GetAxis("Horizontal")) != Mathf.Sign(horizontalVelocity))
                horizontalVelocity *= Mathf.Pow(1 - turningDamping, Time.deltaTime * 10f);
            else
                horizontalVelocity *= Mathf.Pow(1 - basicDamping, Time.deltaTime * 10f);
            rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
            #endregion

            #region vertical
            isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.01f, ground);
            if (isGrounded)
            {
                timeSinceGround = fallingJumpTimer;
                wallJumpCurrentEnergy = wallJumpTotalEnergy;
            }
            else
            {
                rb.velocity += Vector2.up * acc;
                acc -= gravity;
            }

            if (timeSinceGround > 0)
            {
                timeSinceGround -= Time.deltaTime;
            }

            if (jumpBuffer > 0)
            {
                jumpBuffer -= Time.deltaTime;
                if (isGrounded || timeSinceGround > 0)
                {
                    acc = -0.2f;
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
            }


            #endregion

            #region walljump
            if (bag.currentAbility == Ability.WallJump)
            {
                if (isFacingRight)
                    wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, ground);
                else
                    wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, ground);

                if (wallCheckHit && !isGrounded && dir != 0 && !rb.IsTouchingLayers(lava))
                {
                    isWallSliding = true;
                    jumpTime = Time.time + wallJumptime;
                }
                else if (jumpTime < Time.time)
                {
                    isWallSliding = false;
                }

                if (isWallSliding)
                {
                    rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, wallSlideSpeed, float.MaxValue));
                }
            }
            #endregion
        }
        else
        {
            bool isFire = false;
            Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 0.3f);
            foreach(Collider2D coll in colls)
            {
                if(coll.gameObject.layer == 9)
                {
                    isFire = true;
                }
            }
            if(bag.currentAbility == Ability.Roots && !isFire)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    acc = -0.1f;
                }
                else
                {
                    if (rb.velocity.y < 0)
                    {
                        acc = 0.5f;
                    }
                    else
                    {
                        acc = 0.05f;
                    }
                }
            }
            else
            {
                if (rb.velocity.y < 0)
                {
                    acc = 0.5f;
                }
                else
                {
                    acc = 0.05f;
                }
            }
            
            
            rb.velocity += Vector2.up * acc;
            #region horizontal
            float horizontalVelocity = rb.velocity.x;
            horizontalVelocity += Input.GetAxisRaw("Horizontal");

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.01f)
                horizontalVelocity *= Mathf.Pow(1 - stoppingDamping, Time.deltaTime * 10f);
            else if (Mathf.Sign(Input.GetAxis("Horizontal")) != Mathf.Sign(horizontalVelocity))
                horizontalVelocity *= Mathf.Pow(1 - turningDamping, Time.deltaTime * 10f);
            else
                horizontalVelocity *= Mathf.Pow(1 - basicDamping, Time.deltaTime * 10f);
            rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
            #endregion
        }

    }


}
