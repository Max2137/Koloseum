using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class wendigoon : MonoBehaviour
{
    public int HP = 5;
    public int maxHP = 5;

    public float speed = 5;

    private bool isFollowing = true;
    private bool isPreparing = false;
    private bool isCharging = false;
    private bool isAstuned = false;

    public Transform PlayerTarget;
    

    // Start is called before the first frame update
    void Start()
    {
        PlayerTarget = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing == true)
        {
            Transorm.LookAt(PlayerTarget);
            Transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}