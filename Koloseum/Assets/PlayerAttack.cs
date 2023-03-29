using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public wendigoon wendigoon;

    public int cooldownAttackQuick;

    public bool isColliding = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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