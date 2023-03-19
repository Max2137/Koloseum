using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 10;
    public string enemyTag = "Enemy";
    public float comboDamageMultiplier = 2f;
    public float comboCooldown = 5f;

    private float lastComboTime = -Mathf.Infinity;

    void Update()
    {
        // Check if left shift and right mouse button are pressed.
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(1))
        {
            // Check if the combo is off cooldown.
            if (Time.time >= lastComboTime + comboCooldown)
            {
                // Deal combo damage to enemies within a certain radius.
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
                foreach (Collider collider in hitColliders)
                {
                    if (collider.gameObject.CompareTag(enemyTag))
                    {
                        HealthSystem healthSystem = collider.gameObject.GetComponent<HealthSystem>();
                        healthSystem.TakeDamage((int)(attackDamage * comboDamageMultiplier));
                    }
                }

                // Start the combo cooldown.
                lastComboTime = Time.time;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision object has the enemy tag.
        if (collision.gameObject.CompareTag(enemyTag))
        {
            // Deal normal damage to enemies.
            HealthSystem healthSystem = collision.gameObject.GetComponent<HealthSystem>();
            healthSystem.TakeDamage(attackDamage);
        }
    }
}