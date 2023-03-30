using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Efekt : MonoBehaviour
{
    public Image barefect;
    public float barLevel;

    public float effect;
    public float maxefect;

    public float constantDecrease;
    public float increase;
    public float decrease;
    
    void Start()
    {
         
    }

   
    void Update()
    {
        barLevel = effect / maxefect;
        barefect.fillAmount = barLevel;

        effect -= constantDecrease;

        effect -= decrease;
        effect += increase;
    }
}
