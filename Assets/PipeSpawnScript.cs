using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    public GameObject pipe;
    public float spawnRate = 2.5f;
    public float timer = 0;
    public float heightOffset = 5;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate(pipe, transform.position, transform.rotation);
        spawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            spawnPipe();
            timer = 0;
        }
    }

    void spawnPipe()
    {
        Instantiate(
            pipe,
            new Vector3(transform.position.x, Random.Range(-heightOffset, heightOffset), 0),
            transform.rotation
        );
    }
}
