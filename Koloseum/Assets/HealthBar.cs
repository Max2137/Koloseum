using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image barImage;
    public float currentValue;
    public float maxValue = 100f;

    private void Start()
    {
        currentValue = maxValue;
        float percentage = currentValue / maxValue;
        barImage.fillAmount = percentage;
    }

    public void Update()
    {
        float percentage = currentValue / maxValue;
        barImage.fillAmount = percentage;
    }

//    public void DecreaseValue(float amount)
//    {
//        currentValue -= amount;
//        if (currentValue < 0f)
//        {
//           currentValue = 0f;
//        }
//        UpdateBar();
//    }
//
//    public void IncreaseValue(float amount)
//    {
//        currentValue += amount;
//        if (currentValue > maxValue)
//        {
//            currentValue = maxValue;
//        }
//        UpdateBar();
//    }
}
