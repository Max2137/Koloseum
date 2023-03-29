using UnityEngine;

public class ProgressBarController : MonoBehaviour
{
    public Transform progressBar;
    public float amount = 0.5f;

    public void OnPositiveAction()
    {
        amount += 0.1f;
        UpdateProgressBar();
    }

    public void OnNegativeAction()
    {
        amount -= 0.1f;
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        amount = Mathf.Clamp01(amount);
        progressBar.localScale = new Vector3(amount, 1f, 1f);
    }
}