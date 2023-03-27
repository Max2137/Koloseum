using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class wendigoon : MonoBehaviour
{
    public int HP = 5;
    public int maxHP = 5;

    public float speed = 5;

    public bool isFollowing = true;
    public bool isPreparing = false;
    public bool isCharging = false;
    public bool isAstuned = false;

    public Transform player;

    public float dashDistance = 5;

    public float preparingCooldown = 60;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing == true)
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
            isCharging = true;

            isPreparing = false;

            preparingCooldown = 60;
        }

        if (isCharging == true)
        {
            Vector3 dashDirection = (player.position - transform.position).normalized;
            transform.Translate(dashDirection * 5 * Time.deltaTime, Space.World);

            Debug.Log("Charging");

            isCharging = false;
        }
    }
}