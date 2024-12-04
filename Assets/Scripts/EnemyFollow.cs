using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player
    public float speed = 3f;  // Movement speed
    public float stoppingDistance = 1f;  // Distance to stop from the player
    public float avoidanceRadius = 1f;  // Radius to avoid other enemies
    public float avoidanceForce = 2f;  // Force to repel enemies from each other
    public float attackDamage = 10f;
    public float attackCooldown = 1f;

    private float lastAttackTime;
    private Rigidbody rb;  // Rigidbody for physics-based movement

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (player == null)
            Debug.LogError("Player Transform not assigned in EnemyFollow script!");
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
            RotateTowardsPlayer(direction);
        }
        else
        {
            AttackPlayer();
            Vector3 direction = (player.position - transform.position).normalized;
            RotateTowardsPlayer(direction);
        }
        AvoidOtherEnemies();
    }

    private void RotateTowardsPlayer(Vector3 direction)
    {
        direction.y = 0;  // Ignore vertical (y-axis) rotation to prevent unnatural head tilting
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f));
        }
    }


    private void AvoidOtherEnemies()
    {
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, avoidanceRadius);
        foreach (Collider enemy in nearbyEnemies)
        {
            if (enemy.gameObject != this.gameObject)
            {
                Vector3 avoidanceDir = transform.position - enemy.transform.position;
                avoidanceDir.y = 0; // Keep the avoidance movement horizontal
                rb.AddForce(avoidanceDir.normalized * avoidanceForce, ForceMode.Force);
            }
        }
    }
    
    private void AttackPlayer()
    {
        float attackRange = stoppingDistance + 0.2f; // Slight buffer to ensure attacks trigger
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogWarning("PlayerHealth component not found on the player!");
            }
            lastAttackTime = Time.time;
        }
    }
    
    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance + 0.2f);
    }
}
