using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public wendigoon wendigoon;

    public float cooldownAttackQuick;

    public bool isColliding = false;

    public Image barImage3;
    public float barLevel;

    public Efekt efekt;

    public float effect1;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        barLevel = 1 -(cooldownAttackQuick / 60);
        barImage3.fillAmount = barLevel;

        if (wendigoon != null)
        {
            cooldownAttackQuick -= 1;

            if (isColliding == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (cooldownAttackQuick <= 0)
                    {
                        wendigoon.QickAttacked();
                        //Debug.Log("Atakuje");

                        cooldownAttackQuick = 250;

                        effect1 = efekt.effect;
                        effect1 += 15;
                        efekt.effect = effect1;

                        efekt.DecreaseSpeedReset();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wendigoon"))
        {
            isColliding = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wendigoon"))
        {
            isColliding = false;
        }
    }
}