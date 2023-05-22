using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Efekt : MonoBehaviour
{
    public bool isWorking = false;
    
    public Image barefect;
    public float barLevel;

[Header("Alleluja")] 
    public float effect; //finalna statystyka
    public float effectMax; //maksymalna warto�� finalnej statystyki
[Range(0,10)]
    public float constantDecrease; //pr�dko�� sta�ego zmniejszania finalnej statystyki
    public float constantDecreaseStart;
    public float constantDecreaseModifier; //pr�dko�� zwi�kszania si� pr�dko�ci sta�ego zmniejszania finalnej statystyki

    public float distanceLongActivator; //g�rna granica dystansu pomi�dzy graczem, a przeciwnikiem po kt�rej przekroczeniu za��cza si� poni�szy modyfikator
    public float distanceLongModifierStart; //modyfikator do sta�ego zmniejszania si� finalnej statystyki, kt�ry za��cza si� po osi�gni�ciu wy�ej napisanego efektu
    private float distanceLongModifier; 

    public float distanceShortActivator; //dolna granica dystansu pomi�dzy graczem, a przeciwnikiem po kt�rej przekroczeniu za��cza si� poni�szy modyfikator
    public float distanceShortModifier; //modyfikator do sta�ego zmniejszania si� finalnej statystyki, kt�ry za��cza si� po osi�gni�ciu wy�ej napisanego efektu

    public float tresholdLowerActivator; //dolna granica pasku finalnego efektu, po kt�rej przekroczeniu za��cza si� poni�szy modyfikator
    public float tresholdLowerModifier; //modyfikator do sta�ego zmniejszania si� finalnej statystyki, kt�ry za��cza si� po osi�gni�ciu wy�ej napisanego efektu

    public float tresholdHigherActivator; //g�rna granica pasku finalnego efektu, po kt�rej przekroczeniu za��cza si� poni�szy modyfikator
    public float tresholdHigherModifier; //modyfikator do sta�ego zmniejszania si� finalnej statystyki, kt�ry za��cza si� po osi�gni�ciu wy�ej napisanego efektu

    private float dashDistanceStart;
    private float dashDistanceEnd;
    public float dashActivatorEdge; //dodatkowy dystans dodawany do zmiennej wykrywaj�cych ni�szy i wy�szy pr�g dystansu gracza z przeciwnikiem, aby umo�liwi� wi�ksze pole zakresu graczowi
    public float dashBoost; //jednorazowy boost do epicko�ci, kt�ry dodaje si� jednorazowo po uciekaj�cym dashu
    public float dashLongDisModifDebuffer; //si�a debuffu zmniejszania si� finalnego efektu w oparciu o zbyt du�y dystans
    public float dashLongDisModifDebTimerStart; //czas, przez kt�ry dzia�a powy�szy efekt
    private float dashLongDisModifDebTimer;
    private bool isDashLongDisModifDebTimerCounting;

    public float distanceAfterChargeActivator; //dystans pomi�dzy graczem, a przeciwnikiem, w chwili kiedy ten ko�czy szar�owanie
    public float distanceAfterChargeBoost; //jednorazowy boost do epicko�ci, kt�ry dodaje si� jednorazowo po osi�gni�ciu wy�ej napisanego efektu
    public float distanceAfterChargeDebuffer; //si�a debuffu zmniejszania si� finalnego efektu w oparciu o zbyt du�y dystans
    public float distanceLongDisModifDebTimerStart; //czas, przez kt�ry dzia�a powy�szy efekt
    private float distanceLongDisModifDebTimer;
    private bool isDistanceLongDisModifDebTimerCounting;

    public float activeBullets;
    public float activeBulletsCooldownStart;
    private float activeBulletsCooldown;
    public float activeBulletsClimaxPoint;
    public float activeBulletsModifier;


    public GameObject CanvasLost;
    public GameObject CanvasLostEpicness;

    public GameObject CanvasLostDeath;

    public GameObject CanvasWin;

    public GameObject CanvasWinLost;

    public Transform playerTransform;
    public Transform wendigoonTransform;
    public Transform golemTransform;
    [SerializeField] private float distance;

    public bool touchingWall;

    public AudioClip audioClip;
    private AudioSource audioSource;

    public float volume = 50f;
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

        dashLongDisModifDebTimer = dashLongDisModifDebTimerStart;
        distanceLongDisModifDebTimer = distanceLongDisModifDebTimerStart;

        activeBulletsCooldown = activeBulletsCooldownStart;

        distanceLongModifier = distanceLongModifierStart;

        touchingWall = false;

        isDashLongDisModifDebTimerCounting = false;
        isDistanceLongDisModifDebTimerCounting = false;

        displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, 0f);


        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.ignoreListenerVolume = true;
    }

   
    void Update()
    {
        //bar update
        barLevel = effect / effectMax;
        barefect.fillAmount = barLevel;

        if (isWorking == true && effect > 0)
        {
            if (wendigoonTransform != null)
            {
                distance = Vector3.Distance(wendigoonTransform.position, playerTransform.position);
            }
            if (golemTransform != null)
            {
                distance = Vector3.Distance(golemTransform.position, playerTransform.position);
            }


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

        if(isDistanceLongDisModifDebTimerCounting == true)
        {
            distanceLongDisModifDebTimer -= 1;
            distanceLongModifier *= distanceAfterChargeDebuffer;
        }
        if(distanceLongDisModifDebTimer < 0)
        {
            isDistanceLongDisModifDebTimerCounting = false;
            distanceLongModifier = distanceLongModifierStart;
        }

        if (isFading)
        {
            float newAlpha = displayText.color.a - (Time.deltaTime / fadeDuration);

            if (newAlpha <= 0f)
            {
                isFading = false;
                displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, 0f);
            }
            else
            {
                displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, newAlpha);
            }
        }

        if (activeBullets > 0)
        {
            activeBulletsCooldown -= 1;
        }
        if (activeBulletsCooldown <= 0)
        {
            activeBulletsCooldown = activeBulletsCooldownStart;
            activeBullets -= 1;
        }

        //effect += activeBulletsModifier / 1000;


        // Zamień procent na wartość z zakresu 0-1
        volume = effect / 100f;

        // Ustaw nową głośność
        audioSource.volume = volume;
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

        if (dashDistanceStart < distanceShortActivator + dashActivatorEdge && dashDistanceEnd > distanceLongActivator - dashActivatorEdge)
        {
            effect += dashBoost;

            dashLongDisModifDebTimer = dashLongDisModifDebTimerStart;
            isDashLongDisModifDebTimerCounting = true;
        }
    }

    public void ChargeEnd()
    {
        if (distance < distanceAfterChargeActivator && touchingWall == true)
        {
            effect += distanceAfterChargeBoost;

            distanceLongDisModifDebTimer = distanceLongDisModifDebTimerStart;
            isDistanceLongDisModifDebTimerCounting = true;

            displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, 1f);
            displayText.text = "It was almost hit!";
            isFading = true;
        }
    }

    public Text displayText;
    
    private float fadeDuration = 1.0f;
    private bool isFading = false;
}
