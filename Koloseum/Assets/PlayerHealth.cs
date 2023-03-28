using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public float increaseDelay = 5f; // Delay in seconds for the progress bar increase

    public ProgressBarController progressBarController;

    private Coroutine progressBarCoroutine; // Store reference to the progress bar coroutine

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int attackDamage)
    {
        health -= attackDamage;

        if (health <= 0)
        {
            Die();
        }
        else if ((float)health / maxHealth <= 0.2f)
        {
            // If the progress bar coroutine is not running, start it
            if (progressBarCoroutine == null)
            {
                progressBarCoroutine = StartCoroutine(IncreaseProgressBar());
            }
        }
        else if ((float)health / maxHealth <= 0.15f)
        {
            progressBarController.OnNegativeAction();
        }
        else
        {
            // If the progress bar coroutine is running, stop it
            if (progressBarCoroutine != null)
            {
                StopCoroutine(progressBarCoroutine);
                progressBarCoroutine = null;
            }
            progressBarController.OnPositiveAction();
        }
    }

    private IEnumerator IncreaseProgressBar()
    {
        while ((float)health / maxHealth <= 0.2f)
        {
            progressBarController.OnPositiveAction();
            yield return new WaitForSeconds(increaseDelay);
        }
        progressBarCoroutine = null;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
