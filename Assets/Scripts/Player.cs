using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Public Fields
    [SerializeField] float speed;
    [SerializeField] float jumpPower;

    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;

    // Private Fields
    Rigidbody2D body;
    Animator animator;

    const float groundCheckRadius = 0.2f;
    float playerScale = 3f;
    float walkSpeedModifier = 0.5f;
    float horizontalValue;

    [SerializeField] bool isGrounded;
    bool isWalking = false;
    bool facingRight = true;
    bool isJumping = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontalValue = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isWalking = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isWalking = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("Jump", true);
            isJumping = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        animator.SetFloat("yVelocity", body.velocity.y);
    }

    void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue, isJumping);
    }

    void GroundCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        
        isGrounded = false;
        if (colliders.Length > 0)
            isGrounded = true;

        animator.SetBool("Jump", !isGrounded);
    }

    void Move(float dir, bool jumpFlag)
    {
        #region Horizontal Movement
        float xVel = dir * speed * 100 * Time.fixedDeltaTime;

        if (isWalking)
        {
            xVel *= walkSpeedModifier;
        }

        Vector2 targetVelocity = new Vector2(xVel, body.velocity.y);
        body.velocity = targetVelocity;

        if (facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-playerScale, playerScale, playerScale);
            facingRight = false;
        }
        else if (!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(playerScale, playerScale, playerScale);
            facingRight = true;
        }

        animator.SetFloat("xVelocity", Mathf.Abs(body.velocity.x));
        #endregion

        #region Jumping

        if (isGrounded && jumpFlag)
        {
            body.AddForce(new Vector2(0f, jumpPower));
            isGrounded = false;
            jumpFlag = false;
        }

        #endregion
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coins"))
        {
            Destroy(collision.gameObject);
        }
    }
}
