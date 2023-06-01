using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // by default, variables are private
    Rigidbody2D rb;
    Animator anim;
    BoxCollider2D box;
    SpriteRenderer sprite;

    float dx = 0;

    [SerializeField]
    LayerMask jumpableGround;

    [SerializeField]
    [Range(5, 9)]
    float moveSpeed = 7;

    [SerializeField]
    [Range(10, 18)]
    float jumpForce = 14;

    [SerializeField]
    AudioSource jumpSoundEffect;

    private enum MovementState
    {
        idle,
        running,
        jumping,
        falling
    };

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();

        // can be done via the UI as well
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        dx = Input.GetAxis("Horizontal");
        dx = Input.GetAxisRaw("Horizontal");

        float dy = Input.GetAxis("Jump");

        Debug.Log("dx: " + dx);
        Debug.Log("dy: " + dy);

        rb.velocity = new Vector2(dx * moveSpeed, rb.velocity.y);

        // if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        updateAnimationState();
    }

    void updateAnimationState()
    {
        MovementState m_state = MovementState.idle;

        if (rb.velocity.y > 0.01f)
        {
            m_state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.01f)
        {
            m_state = MovementState.falling;
        }
        else if (dx > 0)
        {
            sprite.flipX = false;
            m_state = MovementState.running;
        }
        else if (dx < 0)
        {
            sprite.flipX = true;
            m_state = MovementState.running;
        }
        else
        {
            m_state = MovementState.idle;
        }

        anim.SetInteger("state", (int)m_state);
    }

    bool isGrounded()
    {
        return Physics2D.BoxCast(
            box.bounds.center,
            box.bounds.size,
            0f,
            Vector2.down,
            0.1f,
            jumpableGround
        );
    }
}
