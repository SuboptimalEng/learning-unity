using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    public GameObject pipe;

    [SerializeField]
    private float timer;

    [SerializeField]
    private float spawnRate;

    [SerializeField]
    private float heightOffset;

    // Start is called before the first frame update
    void Start()
    {
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
