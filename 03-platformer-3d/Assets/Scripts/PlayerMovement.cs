using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    AudioSource jumpSound;

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
        // audio = GetComponent<AudioSource>();

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
        rb.velocity = v;

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            // v.y = jumpForce;
            Jump();
        }
    }

    private void Jump()
    {
        Vector3 v = rb.velocity;
        v.y = jumpForce;
        rb.velocity = v;
        jumpSound.Play();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy Head"))
        {
            Destroy(other.transform.parent.gameObject);
            Jump();
        }
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
    }
}
