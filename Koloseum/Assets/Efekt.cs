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
    public float maxefect;

    public float constantDecrease;
    public float constantDecreaseStart;

    public GameObject CanvasLost;
    public GameObject CanvasLostEpicness;

    public GameObject CanvasLostDeath;

    public GameObject CanvasWin;

    public GameObject CanvasWinLost;



    public Transform playerTransform;
    public Transform wendigoonTransform;
    public float distance;

    //public float distanceModifier;

    public void start()
    {
        isWorking = true;
    }

    void Start()
    {
        constantDecrease = constantDecreaseStart;
        CanvasLost.SetActive(false);
        CanvasLostEpicness.SetActive(false);
        CanvasWin.SetActive(false);
        CanvasWinLost.SetActive(false);
    }

   
    void Update()
    {
        //bar update
        barLevel = effect / maxefect;
        barefect.fillAmount = barLevel;

        if (isWorking == true && effect > 0)
        {
            distance = Vector3.Distance(wendigoonTransform.position, playerTransform.position);

            effect = effect - constantDecrease;
            constantDecrease = (float)(constantDecrease * 1.0025);

            if (distance > 13)
            {
                effect -= (float)(distance * 0.0005);
            }
            if (distance < 5)
            {
                effect += (float)(distance * 0.001);
            }
        }

        if(effect < 0)
        {
            effect = 0;
            CanvasLost.SetActive(true);
            CanvasLostEpicness.SetActive(true);
            Time.timeScale = 0f;
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

}
