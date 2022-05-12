using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float AmountOfDamage;


    public GameObject impactVFX;

    private bool collided;

    void OnCollisionEnter(Collision co)
    {

        if (co.gameObject.tag == "Player" && !collided)
        {
            collided = true;

            var impact = Instantiate(impactVFX, co.contacts[0].point, Quaternion.identity) as GameObject;

            Destroy(impact, 2);

            Destroy(gameObject);
        }
    }
}
