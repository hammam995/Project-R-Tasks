using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HamamPlayer : HamamCharacter
{

   
    protected override void Start()
    {
        base.Start();
        delmovemen = movementall;
        starthealth = 100;
        health = starthealth;

    }

    private void FixedUpdate()
    {
        if(delmovemen!= null)
        {
            delmovemen();


        }

    }

    void Update()
    {

       // movementall();

        
    }

    protected void takedamage(float amount) // if a character have a damage
    {
        health -= amount;
        healthbar.fillAmount = health / starthealth;
        Debug.Log("Player health == " + health);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Bullet")
        {
            takedamage(other.gameObject.GetComponent<Bullet>().AmountOfDamage);
            Destroy(other);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Bullet")
        {
            takedamage(collision.gameObject.GetComponent<Bullet>().AmountOfDamage);
            Destroy(collision.gameObject);
        }


    }

}
