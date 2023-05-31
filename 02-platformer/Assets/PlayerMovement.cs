using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D myRigedBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigedBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myRigedBody.velocity = new Vector3(0, 14, 0);
        }
    }
}
