using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private bool isTouchingPlayer;
    public Transform player;
    public PlayerMovement playerMovement;
    public float hitPushbackForce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouchingPlayer == true)
        {
            Rigidbody otherRigidbody = player.gameObject.GetComponent<Rigidbody>();
            if (otherRigidbody != null)
            {
                // Calculate the direction of the pushback effect
                Vector3 pushbackDirection = player.transform.position - transform.position;
                pushbackDirection.Normalize();

                // Apply the pushback force to the player
                otherRigidbody.AddForce(pushbackDirection * hitPushbackForce, ForceMode.Impulse);

                playerMovement.forceStop();

                Debug.Log("Push!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }
}
