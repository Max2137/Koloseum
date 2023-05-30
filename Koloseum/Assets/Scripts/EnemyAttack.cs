using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 20;
    public float pushForce = 5f;
    public float cooldown = 1f;

    private bool canAttack = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            HealthSystem playerHealth = collision.gameObject.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
                pushDirection.y = 0f; // only push back horizontally
                Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
                if (playerRigidbody != null)
                {
                    playerRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
                }
                canAttack = false;
                Invoke(nameof(ResetAttack), cooldown);
            }
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
    }
}
