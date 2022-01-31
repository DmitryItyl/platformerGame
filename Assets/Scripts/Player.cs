using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Public Fields
    float speed = 2f;

    // Private Fields
    Rigidbody2D body;
    Animator animator;
    float playerScale = 3f;
    float walkSpeedModifier = 0.5f;
    float horizontalValue;
    bool isWalking = false;
    bool facingRight = true;

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
    }

    void FixedUpdate()
    {
        Move(horizontalValue);
    }

    void Move(float dir)
    {
        float xVel = dir * speed * 100 * Time.fixedDeltaTime;

        if (isWalking)
        {
            xVel *= walkSpeedModifier;
        }

        Vector2 targetVelocity = new Vector2(xVel, body.velocity.y);
        body.velocity = targetVelocity;

        if (facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-playerScale , playerScale, playerScale);
            facingRight = false;
        }
        else if (!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(playerScale, playerScale, playerScale);
            facingRight = true;
        }

        animator.SetFloat("xVelocity", Mathf.Abs(body.velocity.x));
    }
}
