using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public Image barImage;
    public float barLevel;

    public GameObject CanvasLost;
    public GameObject CanvasLostDeath;
    public Efekt efekt;

    public float effect1;

    public bool isColliding;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        CanvasLostDeath.SetActive(false);


        displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, 0f);

        isColliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        barLevel = hp / maxHp;
        barImage.fillAmount = barLevel;
            

        if (hp <= 0)
        {
            playerDeath();
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
    }

    public void playerDeath()
    {
        efekt.End();
        efekt.Death();
        Destroy(gameObject);
    }
    
    public void WendigoonCharge()
    {
        hp -= 40;

        effect1 = efekt.effect;
        effect1 += 15;
        efekt.effect = effect1;

        efekt.DecreaseSpeedReset();


        displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, 1f);
        displayText.text = "It got HIT!";
        isFading = true;
    }
    public void GolemHit()
    {
        if (isColliding == true)
        {
            hp -= 40;

            effect1 = efekt.effect;
            effect1 += 5;
            efekt.effect = effect1;

            //efekt.DecreaseSpeedReset();


            displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, 1f);
            displayText.text = "It got HIT!";
            isFading = true;
        }
    }


    public Text displayText;
    public float fadeDuration = 1.0f;

    private bool isFading = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wendigoon") || other.CompareTag("Golem"))
        {
            isColliding = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wendigoon") || other.CompareTag("Golem"))
        {
            isColliding = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            hp -= 10;
        }
    }
}
