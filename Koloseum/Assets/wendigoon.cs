using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wendigoon : MonoBehaviour
{
    public float fixedYValue = 1.73f;

    public float HP = 100;
    public float maxHP = 100;

    public float speed = 5;

    public bool isFollowing = true;
    public bool isPreparing = false;
    public bool isCharging = false;
    public bool isStunned = false;

    public Transform player;

    public float preparingCooldown = 60;

    public float dashForce = 1000f;
    public float dashDuration = 1f;
    public float dashDistance = 10;
    private Rigidbody rb;

    public float pushbackForce = 30f;

    public float stunningDuration = 2f;

    public bool touchingWall = false;

    public int damage = 40;
    public bool touchingPlayer = false;

    public bool chargingOngoing = false;

    public bool canPush = true;

    public PlayerMovement playerMovement;

    public PlayerHealth playerHealth;

    public bool canMove = false;

    public Image barImage2;
    public float barLevel;

    public Efekt efekt;


    public AudioClip audioClip;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

        transform.Rotate(new Vector3(0f, 180f, 0f));


        displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, 0f);


        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        barLevel = HP / maxHP;
        barImage2.fillAmount = barLevel;

        if (player != null && canMove == true)
        {

            transform.position = new Vector3(transform.position.x, fixedYValue, transform.position.z);

            if (isFollowing == true && isStunned == false)
            {
                transform.LookAt(player);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer < dashDistance && isFollowing == true)
            {
                isPreparing = true;

                isFollowing = false;
            }

            if (isPreparing == true)
            {
                preparingCooldown -= 1;
            }

            if (preparingCooldown < 0)
            {
                isPreparing = false;

                isCharging = true;

                preparingCooldown = 60;
            }

            if (isCharging == true)
            {
                canPush = true;

                rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);

                isCharging = false;

                chargingOngoing = true;

                Invoke("chargeStop", dashDuration);
            }

            if (isStunned == true)
            {
                //GetComponent<Rigidbody>().isKinematic = true;

                rb.velocity = Vector3.zero;

                // Get the original rotation of the transform
                Quaternion originalRotation = transform.rotation;

                // Create a new Vector3 with the y-axis rotation of the original transform and zero values for x and z
                Vector3 newRotation = new Vector3(0f, originalRotation.eulerAngles.y, 0f);

                // Set the rotation of the transform to the new Vector3
                transform.rotation = Quaternion.Euler(newRotation);
            }

            if (touchingPlayer == true && chargingOngoing == true)
            {
                playerHealth.WendigoonCharge();

                touchingPlayer = false;
                chargingOngoing = false;
            }

            if (HP <= 0)
            {
                Death();
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
    }

    void chargeStop()
    {
        rb.velocity = Vector3.zero;

        chargingOngoing = false;

        efekt.ChargeEnd();

        if (touchingWall == true)
        {
            isStunned = true;
            chargingOngoing = false;


            audioSource.clip = audioClip;

            audioSource.Play();


            Invoke("stunningStop", stunningDuration);
        }
        else
        {
            isFollowing = true;
        }
    }

    void stunningStop()
    {
        isStunned = false;
        touchingWall = false;
        efekt.touchingWall = touchingWall;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        GetComponent<Rigidbody>().isKinematic = false;

        if (distanceToPlayer < dashDistance)
        {
            transform.LookAt(player);
            preparingCooldown = 30;
            isPreparing= true;
        }
        else
        {
            isFollowing = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && isStunned == false && canPush == true) // Check if the other object is the player
        {
            Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();
            if (otherRigidbody != null) // Check if the player has a rigidbody component
            {
                // Calculate the direction of the pushback effect
                Vector3 pushbackDirection = other.transform.position - transform.position;
                pushbackDirection.Normalize();

                // Apply the pushback force to the player
                otherRigidbody.AddForce(pushbackDirection * pushbackForce, ForceMode.Impulse);

                playerMovement.forceStop();
            }

            touchingPlayer = true;
            canPush = false;
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            touchingWall = true;
            efekt.touchingWall = touchingWall;
        }
    }

    public void QickAttacked()
    {
        if (isStunned == true)
        {
            HP -= 40;


            displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, 1f);
            displayText.text = "It got HIT!";
            isFading = true;
        }
    }

    public void DashAttacked()
    {
        HP -= 50;
        isStunned = true;
        chargingOngoing = false;
        Invoke("stunningStop", stunningDuration);

        displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, 1f);
        displayText.text = "It got HIT so hard!";
        isFading = true;
    }

    public void Death()
    {
        efekt.End();
        efekt.Kill();
        Destroy(gameObject);
    }

    public void StartMove()
    {
        canMove = true;
    }


    void OnTriggerEnter(Collider other)
    {
        // Check if the other collider has the tag "Player"
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            HP -= 3;
        }
    }


    public Text displayText;
    public float fadeDuration = 1.0f;

    private bool isFading = false;
}