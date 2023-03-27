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

            isCharging = false;
            isPreparing = false;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < dashDistance && isFollowing == true && isStunned == false)
        {
            isPreparing = true;

            isFollowing = false;
        }

        if (isPreparing == true && isStunned == false)
        {
            preparingCooldown -= 1;
        }

        if (preparingCooldown < 0 && isStunned == false)
        {
            isPreparing = false;

            isCharging = true;

            preparingCooldown = 60;
        }

        if (isCharging == true && isStunned == false)
        {
            rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);

            isCharging = false;

            Invoke("chargeStop", dashDuration);
        }

        if (isStunned == true)
        {
            rb.velocity = Vector3.zero;

            Invoke("stunningStop", stunningDuration);
        }
    }
    
    void chargeStop()
    {
        rb.velocity = Vector3.zero;

        isFollowing = true;
    }

    void stunningStop()
    {
        isStunned = false;

        isFollowing = true;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < dashDistance)
        {
            preparingCooldown = 5;
        }

        if (distanceToPlayer >= dashDistance)
        {
            preparingCooldown = 60;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) // Check if the other object is the player
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
        }
        
        if (other.gameObject.CompareTag("Wall"))
        {
            isStunned = true;

            isFollowing = false;
        }
    }
}