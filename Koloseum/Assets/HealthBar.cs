using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image barImage; // Reference to the image component of the bar
    public float currentValue; // Current value of the bar
    public float maxValue = 100f; // Maximum value of the bar

    private void Start()
    {
        currentValue = maxValue; // Initialize the current value to the maximum value at the start
        UpdateBar(); // Update the bar to show the initial value
    }

    // Method to update the bar value and display it
    private void UpdateBar()
    {
        float percentage = currentValue / maxValue; // Calculate the percentage of the current value
        barImage.fillAmount = percentage; // Set the fill amount of the image to the percentage
    }

    // Method to decrease the bar value by a given amount
    public void DecreaseValue(float amount)
    {
        currentValue -= amount;
        if (currentValue < 0f)
        {
            currentValue = 0f; // Clamp the value to 0 if it goes below
        }
        UpdateBar();
    }

    // Method to increase the bar value by a given amount
    public void IncreaseValue(float amount)
    {
        currentValue += amount;
        if (currentValue > maxValue)
        {
            currentValue = maxValue; // Clamp the value to the maximum if it goes above
        }
        UpdateBar();
    }
}
