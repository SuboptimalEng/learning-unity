using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField]
    GameObject[] waypoints;

    [SerializeField]
    float speed = 1.0f;

    int currentWaypointIndex;

    void Update()
    {
        if (
            Vector3.Distance(transform.position, waypoints[currentWaypointIndex].transform.position)
            < 0.1f
        )
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            waypoints[currentWaypointIndex].transform.position,
            speed * Time.deltaTime
        );
    }
}
