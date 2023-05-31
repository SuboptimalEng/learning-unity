using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // by default, variables are private
    Rigidbody2D rb;
    Animator anim;

    float dx = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

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

        rb.velocity = new Vector2(dx * 7, rb.velocity.y);

        // if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, 14);
        }

        updateAnimationState();
    }

    void updateAnimationState()
    {
        if (dx > 0)
        {
            anim.SetBool("isRunning", true);
        }
        else if (dx < 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }
}
