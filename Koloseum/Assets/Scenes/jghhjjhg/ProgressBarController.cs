using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    // Referencja do komponentu Image paska postêpu
    public Image progressBarImage;

    // Pocz¹tkowa wartoœæ paska postêpu
    private float amount = 0.5f;

    // Funkcja wywo³ywana, gdy gracz wykonuje pozytywn¹ akcjê
    public void OnPositiveAction()
    {
        amount += 0.1f;
        UpdateProgressBar();
    }

    // Funkcja wywo³ywana, gdy gracz wykonuje negatywn¹ akcjê
    public void OnNegativeAction()
    {
        amount -= 0.1f;
        UpdateProgressBar();
    }

    // Funkcja aktualizuj¹ca wartoœæ paska postêpu i wywo³ywana po ka¿dej zmianie wartoœci
    private void UpdateProgressBar()
    {
        // Ograniczenie wartoœci amount do przedzia³u 0 - 1
        amount = Mathf.Clamp01(amount);
        
        // Aktualizacja wartoœci fillAmount komponentu Image
        progressBarImage.fillAmount = amount;
    }
}
