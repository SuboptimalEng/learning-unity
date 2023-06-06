using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    [Range(4, 12)]
    int jumpForce = 8;

    [SerializeField]
    [Range(4, 12)]
    int movementSpeed = 8;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = rb.velocity;

        float horizontalInput = Input.GetAxis("Horizontal");
        // vertical up down should move in z axis
        float verticalInput = Input.GetAxis("Vertical");

        v.x = horizontalInput * movementSpeed;
        v.z = verticalInput * movementSpeed;

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            v.y = jumpForce;
        }

        rb.velocity = v;

        // if (Input.GetKeyDown("space"))
        // {
        //     rb.velocity = Vector3.up * jumpForce;
        // }
        // if (Input.GetKey("up"))
        // {
        //     rb.velocity = Vector3.forward * movementSpeed;
        // }
        // if (Input.GetKey("down"))
        // {
        //     rb.velocity = Vector3.back * movementSpeed;
        // }
        // if (Input.GetKey("left"))
        // {
        //     rb.velocity = Vector3.left * movementSpeed;
        // }
        // if (Input.GetKey("right"))
        // {
        //     rb.velocity = Vector3.right * movementSpeed;
        // }
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
    }
}
