using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField]
    private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    [SerializeField]
    private float speed = 2f;

    void Update()
    {
        if (
            Vector2.Distance(transform.position, waypoints[currentWaypointIndex].transform.position)
            < 0.1f
        )
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            waypoints[currentWaypointIndex].transform.position,
            Time.deltaTime * speed
        );
    }
}
