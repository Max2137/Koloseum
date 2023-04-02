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

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        CanvasLostDeath.SetActive(false);
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
    }
}
