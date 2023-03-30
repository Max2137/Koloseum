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


    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
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
        Destroy(gameObject);
    }
    
    public void WendigoonCharge()
    {
        hp -= 40;
    }
}
