using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public float flapStrength;
    public LogicManagerScript logic;

    public bool birdIsAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        // gameObject.name = "Bob Birdington";
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(S))
        if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
        {
            myRigidBody.velocity = Vector2.up * flapStrength;
        }

        if (transform.position.y > 13 || transform.position.y < -13)
        {
            gameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        gameOver();
    }

    private void gameOver()
    {
        logic.gameOver();
        birdIsAlive = false;
    }
}
