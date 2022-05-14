using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testingr : MonoBehaviour
{

    public int digit;
    public string CurrrentState;
    public int minShield;
    public int maxShield; 
    public int minExplosion;
    public int maxEplosion;
    public int minTired;
    public int maxTired;

    public int ShieldPercentage;
    public int ExplosionPercentage;
    public int TiredPercentage;
    void Start()
    {
        minShield = 0;
        maxShield = ShieldPercentage;
        minExplosion = maxShield + 1;
        maxEplosion = minExplosion+ExplosionPercentage;
        minTired = maxEplosion + 1;
        maxTired = minTired+TiredPercentage;
        // depends on the states we increase it , or for better solution start from minus

        InvokeRepeating("State", 2, 2);
    }

    void Update()
    {
    }

    public void State()
    {
        digit = Random.Range(0, 102);
        Debug.Log("Curre " + digit);
        if (digit >= minShield && digit <= maxShield)
            CurrrentState = "1 is shield";
        if (digit >= minExplosion && digit <= maxEplosion)
            CurrrentState = "2 is EXP";
        if (digit >= minTired && digit <= maxTired)
            CurrrentState = "3 is tired";
       
    }

    /*  digit = Random.Range(0, 102);
          Debug.Log("Curre " + digit);

          if (digit >= 0 && digit <= 50)
              CurrrentState = "1 is shield";
          if(digit>=51 && digit<=81)
              CurrrentState = "2 is EXP";
          if (digit >= 82 && digit <= 102)
              CurrrentState = "3 is tired";
              */
}
