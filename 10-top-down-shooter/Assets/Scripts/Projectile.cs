using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask collisionMask;
    float speed = 10;
    float damage = 1;

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    void Update()
    {
        float moveDistance = speed * Time.deltaTime;

        CheckCollisions(moveDistance);

        transform.Translate(Vector3.forward * moveDistance);
    }

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (
            Physics.Raycast(
                ray,
                out hit,
                moveDistance,
                collisionMask,
                QueryTriggerInteraction.Collide
            )
        )
        {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit);
        }

        // print(hit.collider.gameObject.name);
        GameObject.Destroy(gameObject);
    }
}
