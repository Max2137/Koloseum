using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public ProgressBarController progressBarController;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
        else if ((float)health / maxHealth <= 0.2f)
        {
            progressBarController.OnPositiveAction();
        }
        else if ((float)health / maxHealth <= 0.15f)
        {
            progressBarController.OnNegativeAction();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
