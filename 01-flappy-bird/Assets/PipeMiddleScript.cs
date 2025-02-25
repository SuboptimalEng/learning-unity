using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMiddleScript : MonoBehaviour
{
    private LogicManagerScript logic;

    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManagerScript>();
    }

    // Update is called once per frame
    void Update() { }

    // note: unable to get intellisense to suggest "OnTriggerEnter2D"
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 3)
        {
            if (other.gameObject.GetComponent<BirdController>().birdIsAlive)
            {
                logic.addScore(1);
            }
        }
    }
}
