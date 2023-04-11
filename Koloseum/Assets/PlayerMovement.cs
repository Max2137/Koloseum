using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 10f;
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public Rigidbody rb;
    private Vector3 movement;
    public bool canDash = true;
    public bool isDashing;
    private bool isFlying = true;
    public bool touchingWall = false;
    public wendigoon wendigoon;
    public EnemyGolem enemyGolem;
    public Efekt efekt;
    public PlayerAttack playerAttack;
    public bool isStunned;
    public float StunnedTimer;
    public float StunnedTimerStart;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        transform.Rotate(new Vector3(0f, -90f, 0f));

        isDashing = false;
        isStunned = false;

        StunnedTimer = StunnedTimerStart;
    }

    void FixedUpdate()
    {
        if (isFlying == false && isStunned == false) 
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            movement = new Vector3(horizontal, 0f, vertical).normalized;
            Vector3 lookDirection = movement.magnitude > 0f ? movement : transform.forward;

            Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            Quaternion newRotation = Quaternion.Lerp(rb.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation);

            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void Update()
    {
        if (canDash && Input.GetKeyDown(KeyCode.Space) && isFlying == false && isStunned == false)
        {
            StartCoroutine(Dash());

            if (wendigoon != null || enemyGolem != null)
            {
                efekt.DashStart();
            }
        }

        if (isStunned == true)
        {
            StunnedTimer -= 1;
        }

        if (StunnedTimer <= 0)
        {
            isStunned = false;
            StunnedTimer = StunnedTimerStart;
        }
    }

    IEnumerator Dash()
    {
        Invoke("DashEndAttack", dashDuration);

        canDash = false;
        isDashing = true;
        Vector3 dashDirection = movement.magnitude > 0f ? movement.normalized : transform.forward;
        float dashDistanceRemaining = dashDistance;
        float dashTimer = 0f;

        while (dashDistanceRemaining > 0f && dashTimer < dashDuration && touchingWall == false)
        {
            float dashDistanceThisFrame = Mathf.Min(dashDistanceRemaining, Time.deltaTime * dashDistance / dashDuration);
            rb.MovePosition(rb.position + dashDirection * dashDistanceThisFrame);
            dashDistanceRemaining -= dashDistanceThisFrame;
            dashTimer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

        if (wendigoon != null || enemyGolem != null)
        {
            efekt.DashEnd();
            isDashing = false;
        }
    }
    public void forceStop()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            //Debug.Log("Touched grass");
            isFlying = false;
            
            if (wendigoon != null)
            {
                wendigoon.StartMove();
                efekt.start();
            }
            if (enemyGolem != null)
            {
                enemyGolem.StartMove();
                efekt.start();
            }
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            touchingWall = true;
            //Debug.Log("Touched wall");
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            touchingWall = false;
            //Debug.Log("Stop touched wall");
        }
    }

    public void DashEndAttack()
    {
        playerAttack.DashEnd();
    }
}
