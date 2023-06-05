using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public wendigoon wendigoon;
    public EnemyGolem enemyGolem;

    public PlayerMovement playerMovement;

    public float cooldownAttackQuick;

    public bool isColliding = false;

    public bool didTouch;
    public bool didHit;

    public Image barImage3;
    public float barLevel;

    public Efekt efekt;

    public float effect1;

    public float hitPushbackForce = 10f;


    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        barLevel = 1 - (cooldownAttackQuick / 60);
        barImage3.fillAmount = barLevel;

        cooldownAttackQuick -= 1;

        if (playerMovement.isDashing == true)
        {
            if (isColliding == true)
            {
                didTouch = true;
            }
            else
            {
                //didTouch = false;
            }
        }
        else
        {
            //didTouch = false;
        }

        if (wendigoon != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (cooldownAttackQuick <= 0)
                {
                    if (isColliding == true)
                    {
                        wendigoon.QickAttacked();
                        //Debug.Log("Atakuje");

                        effect1 = efekt.effect;
                        effect1 += 15;
                        efekt.effect = effect1;

                        efekt.DecreaseSpeedReset();
                    }

                    
                  
                }

                cooldownAttackQuick = 250;
            }
        }

        if (enemyGolem != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (cooldownAttackQuick <= 0)
                {
                    if (isColliding == true)
                    {
                        enemyGolem.QickAttacked();
                        //Debug.Log("Atakuje");

                        effect1 = efekt.effect;
                        effect1 += 5;
                        efekt.effect = effect1;

                        //efekt.DecreaseSpeedReset();
                    }

                    if (playerMovement.isDashing == true)
                         {
                             if (isColliding == true)
                             {
                                 enemyGolem.QickAttacked();
                                 Debug.Log("Atakuje");

                                 effect1 = efekt.effect;
                                 effect1 += 5;
                                 efekt.effect = effect1;

                                 //efekt.DecreaseSpeedReset();
                             }
                         }
                         else
                         {
                             didHit = true;
                         }
                    
                }

                cooldownAttackQuick = 250;
            }
        }


        /*    if (Input.GetMouseButtonDown(1))
            {
                efekt.activeBullets += 1;


                Vector3 spawnPosition = transform.position + transform.forward * 1f + Vector3.up * 1f; // Spawn the bullet 1 meter in front and 1 meter above the player
                GameObject bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation); // Spawn the bullet prefab at the calculated position

                // Add forward force to the bullet
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 1000);

                if (efekt.activeBullets < 1)
                {
                    efekt.activeBulletsModifier = 0;
                }
                else if (efekt.activeBullets < efekt.activeBulletsClimaxPoint)
                {
                    //positive effect
                    efekt.effect += efekt.activeBullets;
                    efekt.constantDecrease = efekt.constantDecrease * -efekt.activeBullets/4;
                }
                else
                {
                    //negative effect
                    efekt.effect -= efekt.activeBullets /2;
                }
            }
    */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wendigoon") || other.CompareTag("Golem"))
        {
            isColliding = true;

            if (didHit == true && wendigoon != null)
            {
                Rigidbody otherRigidbody = wendigoon.gameObject.GetComponent<Rigidbody>();
                if (otherRigidbody != null)
                {
                    // Calculate the direction of the pushback effect
                    Vector3 pushbackDirection = wendigoon.transform.position - transform.position;
                    pushbackDirection.Normalize();

                    // Apply the pushback force to the player
                    otherRigidbody.AddForce(pushbackDirection * hitPushbackForce, ForceMode.Impulse);

                    //Debug.Log("Push!");
                }
            }
        }

        
        if (other.CompareTag("Wendigoon") || other.CompareTag("Golem")) {
            if (playerMovement.isDashing == true)
            {
                if (isColliding == true)
                {
                    
                    other.GetComponent<IMonster>().Stun();
                    if (other.CompareTag("Golem"))
                        other.GetComponent<IMonster>().QickAttacked(10);
                    else 
                        other.GetComponent<IMonster>().QickAttacked(80);
                    Debug.Log("Atakuje");

                    effect1 = efekt.effect;
                    effect1 += 1;
                    efekt.effect = effect1;

                    //efekt.DecreaseSpeedReset();
                }
            }
            else
            {
                didHit = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wendigoon") || other.CompareTag("Golem"))
        {
            isColliding = false;
        }
    }

    public void DashEnd()
    {
        /* if (didHit && didTouch)
         {
             //Debug.Log("Dashowy atak");

             if (wendigoon != null)
             {
                 wendigoon.DashAttacked();

                 effect1 = efekt.effect;
                 effect1 += 25;
                 efekt.effect = effect1;

                 efekt.DecreaseSpeedReset();
             }

             if (enemyGolem != null)
             {
                 enemyGolem.DashAttacked();

                 effect1 = efekt.effect;
                 effect1 += 25;
                 efekt.effect = effect1;

                 efekt.DecreaseSpeedReset();
             }

             playerMovement.isStunned = true;

         }
        */
        didHit = false;
        didTouch = false;
    }
}