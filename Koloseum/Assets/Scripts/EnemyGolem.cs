using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class EnemyGolem : MonoBehaviour, IMonster
{
    public Transform player;
    private float fixedYValue = 0.8f;
    private float distance;
    private bool isStarted;
    private bool isTouchingPlayer;

    
    public bool isStunned = false;
    public float stunningDuration = 2f;


    private float hp;
    public float hpMax;

    public Image barImage2;
    public float barLevel;


    private bool isFollowing;
    public float speed;

    private bool isHitting;
    public float hitDistance;
    public float hitPushbackForce;
    private float hitCooldown;
    public float hitCooldownMax;
    private bool isHitPreparing;
    public int numRocks;
    public int numRocksIncrease;
    public int numRocksStart;
    public float radius;
    public float radiusIncrease;
    public float radiusStart;
    public int numCircles;
    int counter = 0;

    private bool isRocking;
    private float rockCooldown;
    public float rockCooldownMax;
    private Vector3 crystalSpawn;
    public GameObject rockFormation;
    public float rockEscapeTime;
    public float rockStatingTime;
    public float betweenCirclesTime;
    public float rockingStart;

    private bool isLining;
    private bool isLinePreparing;
    public float lineDistance;
    public float lineCooldownMax;
    private float lineCooldown;
    private float distanceFromEnemy;
    public float distanceFromEnemyStart;
    public float lineRocksNum;
    public float betweenLineRocksTime;
    private int rocksInLineCounter;

    public float spawnInterval;
    private Vector3 playerPosition;

    public PlayerMovement playerMovement;
    public PlayerHealth playerHealth;
    public Efekt efekt;

    // Start is called before the first frame update
    void Start()
    {
        hp = hpMax;
        
        isStarted = false;
        isTouchingPlayer = false;
        isFollowing = true;

        hitCooldown = hitCooldownMax;
        rockCooldown = rockCooldownMax;
        lineCooldown = lineCooldownMax;
        distanceFromEnemy = distanceFromEnemyStart;

        radius = radiusStart;
        numRocks = numRocksStart;

        InvokeRepeating("spawnRock", spawnInterval, spawnInterval);
        InvokeRepeating("PlayerPositionActualization", 0, rockEscapeTime);
    }

    public void Stun() {
        isStunned = true;
        Invoke("stunningStop", stunningDuration + 10f);
    }

    void stunningStop() {
        isStunned = false;
    }

    public void QickAttacked(int DealHP = 40)
    {
        hp -= DealHP;
    }

    public void StartMove()
    {
        isStarted = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        barLevel = hp / hpMax;
        barImage2.fillAmount = barLevel;

        transform.position = new Vector3(transform.position.x, fixedYValue, transform.position.z);
        if (player == null) return;


        distance = Vector3.Distance(transform.position, player.position);

        isTouchingPlayer = playerHealth.isColliding;

        if (hp <= 0)
        {
            efekt.End();
            efekt.Kill();
            Destroy(gameObject);
        }

        if (isStunned == true)
        {
            Quaternion originalRotation = transform.rotation;

            // Create a new Vector3 with the y-axis rotation of the original transform and zero values for x and z
            Vector3 newRotation = new Vector3(0f, originalRotation.eulerAngles.y, 0f);

            // Set the rotation of the transform to the new Vector3
            transform.rotation = Quaternion.Euler(newRotation);
            return;
        }

        if (isStarted == true)
        {
            if (isFollowing == true && isStunned == false)
            {
                transform.LookAt(player);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);

                if (distance < hitDistance)
                {
                    isFollowing = false;
                    isHitPreparing = true;
                    isRocking = false;
                }

                if (distance > lineDistance)
                {
                    isLinePreparing = true;
                }
                else
                {
                    isLinePreparing = false;
                    lineCooldown = lineCooldownMax;
                }
                if (isLinePreparing == true)
                {
                    lineCooldown -= 1;
                }
                if (lineCooldown <= 0)
                {
                    lineCooldown = lineCooldownMax;

                    //Debug.Log("Line!");
                    StartCoroutine(SpawnRockLine());
                }
            }
            else
            {
                if (isHitPreparing == true)
                {
                    hitCooldown -= 1;
                }
                if (hitCooldown <= 0)
                {
                    hitCooldown = hitCooldownMax;
                    isHitPreparing = false;

                    playerHealth.GolemHit();

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

                            //Debug.Log("Push!");
                        }
                    }

                    Invoke("StartRockRingSpawn", rockingStart);
                }
            }

            

            if (distance > hitDistance)
            {
                isRocking = true;
            }

            if (counter == numCircles)
            {
                counter = 0;
                Invoke("lastCircleVanished", rockStatingTime);
            }

            if (rocksInLineCounter == lineRocksNum)
            {
                distanceFromEnemy = distanceFromEnemyStart;
                rocksInLineCounter = 0;
                isFollowing = true;

                //Invoke("A");
            }
        }
    }
    public void spawnRock()
    {
        if (isStunned == true) {
            return;
        }

        if (isRocking && isFollowing)
        {
            playerPosition.y = 1;
            GameObject Rock = Instantiate(rockFormation, playerPosition, Quaternion.identity);

            Destroy(Rock, rockStatingTime);
        }
    }
    public void PlayerPositionActualization()
    {
        if (player == null) return;

        playerPosition = player.transform.position;
    }

    public void StartRockRingSpawn()
    {
        StartCoroutine(SpawnRockRing());
    }
    private IEnumerator SpawnRockRing()
    {
        if (isStunned == true) {
            yield return null;
        }

        Vector3 center = transform.position; // Get player's position

        for (int z = 0; z < numCircles; z++)
        {
            for (int i = 0; i < numRocks; i++)
            {
                // Calculate the angle for each rock
                float angle = i * Mathf.PI * 2 / numRocks;

                // Calculate the position of the rock in polar coordinates
                float x = center.x + radius * Mathf.Cos(angle);
                float zPos = center.z + radius * Mathf.Sin(angle); // Rename the variable to zPos
                Vector3 position = new Vector3(x, center.y, zPos);

                // Spawn the rock at the calculated position
                GameObject rock = Instantiate(rockFormation, position, Quaternion.identity);

                // Destroy the rock after 5 seconds
                Destroy(rock, rockStatingTime);
            }
            numRocks += numRocksIncrease;
            radius += radiusIncrease;
            counter++;
            yield return new WaitForSecondsRealtime((float)betweenCirclesTime); // Add a 1 second delay before next loop iteration
        }
    }
    public void lastCircleVanished()
    {
        isFollowing = true;

        radius = radiusStart;
        numRocks = numRocksStart;
    }


    private IEnumerator SpawnRockLine()
    {
        if (isStunned == true) {
            yield return null;
        }

        PlayerPositionActualization();

        isFollowing = false;

        Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;

        for (int x = 0; x < lineRocksNum; x++)
        {
            Vector3 enemyPosition = transform.position;

            Vector3 direction = playerPosition - enemyPosition;
            direction.y = 0; // ensure the formation is at the same height as the enemy

            Vector3 formationPosition = enemyPosition + direction.normalized * distanceFromEnemy;

            GameObject rock = Instantiate(rockFormation, formationPosition, Quaternion.identity);

            distanceFromEnemy += 3;

            Destroy(rock, rockStatingTime);

            yield return new WaitForSecondsRealtime((float)betweenLineRocksTime);

            rocksInLineCounter++;
        }
    }

    public void DashAttacked()
    {
        //displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, 1f);
        //displayText.text = "It got HIT so hard!";
        //isFading = true;

        hp -= 50;
    }
}