using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class wendigoon : MonoBehaviour
{
    public float fixedYValue = 1.73f;

    public int HP = 5;
    public int maxHP = 5;

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

    private PlayerHealth playerHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
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
            rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);

            isCharging = false;

            chargingOngoing = true;

            Invoke("chargeStop", dashDuration);
        }

        if (isStunned == true)
        {
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
            Debug.Log("-40hp");
            touchingPlayer = false;
            chargingOngoing = false;
        }
    }

    void chargeStop()
    {
        rb.velocity = Vector3.zero;

        chargingOngoing = false;

        if (touchingWall == true)
        {
            isStunned = true;
            chargingOngoing = false;
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

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

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
        if (other.gameObject.CompareTag("Player") && isStunned == false) // Check if the other object is the player
        {
            Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();
            if (otherRigidbody != null) // Check if the player has a rigidbody component
            {
                // Calculate the direction of the pushback effect
                Vector3 pushbackDirection = other.transform.position - transform.position;
                pushbackDirection.Normalize();

                // Apply the pushback force to the player
                otherRigidbody.AddForce(pushbackDirection * pushbackForce, ForceMode.Impulse);
            }

            touchingPlayer = true;
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            touchingWall = true;
        }
    }
}