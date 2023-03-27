using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    // Referencja do komponentu Image paska post�pu
    public Image progressBarImage;

    // Pocz�tkowa warto�� paska post�pu
    private float amount = 0.5f;

    // Funkcja wywo�ywana, gdy gracz wykonuje pozytywn� akcj�
    public void OnPositiveAction()
    {
        amount += 0.1f;
        UpdateProgressBar();
    }

    // Funkcja wywo�ywana, gdy gracz wykonuje negatywn� akcj�
    public void OnNegativeAction()
    {
        amount -= 0.1f;
        UpdateProgressBar();
    }

    // Funkcja aktualizuj�ca warto�� paska post�pu i wywo�ywana po ka�dej zmianie warto�ci
    private void UpdateProgressBar()
    {
        // Ograniczenie warto�ci amount do przedzia�u 0 - 1
        amount = Mathf.Clamp01(amount);
        
        // Aktualizacja warto�ci fillAmount komponentu Image
        progressBarImage.fillAmount = amount;
    }
}
