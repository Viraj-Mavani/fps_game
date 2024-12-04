using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    public float maxDistance = 1000f;
    public GameObject decalHitWall;
    public GameObject bloodEffect;
    public float floatInFrontOfWall = 0.05f;
    public float bulletDamage = 10f;  // Example damage value
    public LayerMask ignoreLayer;
    public float bulletSpeed = 50f;  // Speed of the bullet

    private RaycastHit hit;
    private Rigidbody rb;

    void Start()
    {
        // Setup Rigidbody for bullet movement
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * bulletSpeed; // Bullet starts moving
        }

        Destroy(gameObject, 5f); // Destroy bullet after 5 seconds if not hit
    }

    void Update()
    {
        if (MouseLookScript.isUIActive) return;

        if (rb.velocity.magnitude > 0)
        {
            PerformRaycast();
        }
    }

    private void PerformRaycast()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, ~ignoreLayer))
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        if (hit.transform.CompareTag("LevelPart"))
        {
            if (decalHitWall != null)
            {
                Instantiate(decalHitWall,
                    hit.point + hit.normal * floatInFrontOfWall,
                    Quaternion.LookRotation(hit.normal));
            }
        }
        else if (hit.transform.CompareTag("Enemy"))
        {
            if (bloodEffect != null)
            {
                Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(bulletDamage);
        }
        else
        {
            Debug.LogWarning("Bullet hit an unhandled object: " + hit.transform.name);
        }

        Destroy(gameObject);
    }
}
