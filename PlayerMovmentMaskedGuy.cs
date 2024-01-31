using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementMaskedGuy : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    [SerializeField]private float moveSpeed = 7f;
    [SerializeField]private float jumpForce = 9f;

    private enum MovementState { idle, running, jumping, falling }

    private float dirX = 0f;
    
    private bool isJumping;
    private bool doubleJump;
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        // Perform jump whenever the "Jump" button is pressed
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isJumping = true;
                doubleJump = !doubleJump;
            }
        }
  
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("State", (int)state);
    }

    private bool IsGrounded()
    {
       return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}