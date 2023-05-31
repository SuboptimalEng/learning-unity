using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // by default, variables are private
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;

    float dx = 0;

    [SerializeField]
    [Range(5, 9)]
    float moveSpeed = 7;

    [SerializeField]
    [Range(10, 18)]
    float jumpForce = 14;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        updateAnimationState();
    }

    void updateAnimationState()
    {
        if (dx > 0)
        {
            sprite.flipX = false;
            anim.SetBool("isRunning", true);
        }
        else if (dx < 0)
        {
            sprite.flipX = true;
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }
}
