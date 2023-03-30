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
            if (cooldownAttackQuick > 0)
            cooldownAttackQuick -= 1;

            if (isColliding == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (cooldownAttackQuick <= 0)
                    {
                        wendigoon.QickAttacked();
                        //Debug.Log("Atakuje");

                        cooldownAttackQuick = 60;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wendigoon"))
        {
            isColliding = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Wendigoon"))
        {
            isColliding = false;
        }
    }
}