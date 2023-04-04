using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Efekt : MonoBehaviour
{
    public bool isWorking = false;
    
    public Image barefect;
    public float barLevel;

    public float effect;
    public float effectMax;

    public float constantDecrease;
    public float constantDecreaseStart;
    public float constantDecreaseModifier;

    public float distanceLongActivator;
    public float distanceLongModifierStart;
    private float distanceLongModifier;

    public float distanceShortActivator;
    public float distanceShortModifier;

    public float tresholdLowerActivator;
    public float tresholdLowerModifier;

    public float tresholdHigherActivator;
    public float tresholdHigherModifier;

    private float dashDistanceStart;
    private float dashDistanceEnd;
    public float dashPremium;
    public float dashLongDisModifDebuffer;
    public float dashLongDisModifDebTimerStart;
    private float dashLongDisModifDebTimer;
    private bool isDashLongDisModifDebTimerCounting;





    public GameObject CanvasLost;
    public GameObject CanvasLostEpicness;

    public GameObject CanvasLostDeath;

    public GameObject CanvasWin;

    public GameObject CanvasWinLost;

    public Transform playerTransform;
    public Transform wendigoonTransform;
    private float distance;

    //public float distanceModifier;

    public void start()
    {
        isWorking = true;

        isDashLongDisModifDebTimerCounting = false;
    }

    void Start()
    {
        constantDecrease = constantDecreaseStart;
        CanvasLost.SetActive(false);
        CanvasLostEpicness.SetActive(false);
        CanvasWin.SetActive(false);
        CanvasWinLost.SetActive(false);

        displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, 0f);

        dashLongDisModifDebTimer = dashLongDisModifDebTimerStart;
        distanceLongModifier = distanceLongModifierStart;
    }

   
    void Update()
    {
        //bar update
        barLevel = effect / effectMax;
        barefect.fillAmount = barLevel;

        if (isWorking == true && effect > 0)
        {
            distance = Vector3.Distance(wendigoonTransform.position, playerTransform.position);

            effect = effect - constantDecrease;
            constantDecrease = (float)(constantDecrease) * (float)(constantDecreaseModifier);

            if (effect < tresholdLowerActivator)
            {
                constantDecrease = (float)(constantDecrease / tresholdLowerModifier);


                displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, 1f);
                displayText.text = "Boo... They're just walking.";
                isFading = true;
            }
            if (effect > tresholdHigherActivator)
            {
                constantDecrease = (float)(constantDecrease / tresholdHigherModifier);
            }

            if (distance > distanceLongActivator)
            {
                effect += (float)(distance * distanceLongModifier);
            }
            if (distance < distanceShortActivator)
            {
                effect += (float)(distanceShortModifier);
            }
        }

        if(effect < 0)
        {
            effect = 0;
            CanvasLost.SetActive(true);
            CanvasLostEpicness.SetActive(true);
            Time.timeScale = 0f;
        }

        if(effect > 100)
        {
            effect = 100;
        }

        if(isDashLongDisModifDebTimerCounting == true)
        {
            dashLongDisModifDebTimer -= 1;
            distanceLongModifier *= dashLongDisModifDebuffer;
        }
        if(dashLongDisModifDebTimer < 0)
        {
            isDashLongDisModifDebTimerCounting = false;
            distanceLongModifier = distanceLongModifierStart;
        }
    }

    public void DecreaseSpeedReset()
    {
        constantDecrease = constantDecreaseStart;
    }

    public void End()
    {
        isWorking = false;
    }
    
    public void Death()
    {
        Invoke("DeathScreen", (float)0.5);
    }
    public void DeathScreen()
    {
        Time.timeScale = 0f;
        CanvasLost.SetActive(true);
        CanvasLostDeath.SetActive(true);
    }

    public void Kill()
    {
        Invoke("WinScreen", (float)0.75);
    }
    public void WinScreen()
    {
        if (effect >= 50)
        {
            CanvasWin.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            CanvasLost.SetActive(true);
            CanvasWinLost.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void DashStart()
    {
        dashDistanceStart = distance;
    }
    public void DashEnd()
    {
        Invoke("DashEndCounting", 1/25);
    }
    public void DashEndCounting()
    {
        dashDistanceEnd = distance;

        if (dashDistanceStart < distanceShortActivator && dashDistanceEnd > distanceLongActivator)
        {
            effect += dashPremium;

            dashLongDisModifDebTimer = dashLongDisModifDebTimerStart;
            isDashLongDisModifDebTimerCounting = true;
        }
    }

    public Text displayText;
    private float fadeDuration = 1.0f;

    private bool isFading = false;
}
