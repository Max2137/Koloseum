using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int hp;
    public int maxHp;
    public ProgressBarController progressBarController;
    
    
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            playerDeath();
        }
    }

    public void playerDeath()
    {
        Destroy(gameObject);
    }
    
    public void WendigoonCharge()
    {
        hp -= 40;
        progressBarController.OnNegativeAction();
        
       
    }
}
