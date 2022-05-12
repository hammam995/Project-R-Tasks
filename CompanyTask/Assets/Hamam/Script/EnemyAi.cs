using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;


public class EnemyAi : MonoBehaviour
{
    public bool StartEnemySystem;

    [Header("Player Attributes")]
    Transform player;
    public float dist;
    public HamamPlayer HamamScript;




    public enum AI_Distance_State { nearDistance, farDistance , idle  };
    public enum Near_State_sitiuation { shield, tired , explosion , idle };

    [Header("States Attributes")]
    public AI_Distance_State aiState = AI_Distance_State.farDistance;
    public Near_State_sitiuation nearState;
    public string CurrentState;


    [Header("Near States Attributes")]
    public float myFloat;
    public int tiempoEntreMens = 5; // the time that which we enter it , and iit does not matter if we change it
    public int counter = 0;

    [Header("Random Probability Near States Attributes")]
    public int digit;
    public string CurrrentState;
    [Range(0, 102)]
    public int minShield;
    [Range(0, 102)]
    public int maxShield;
    [Range(0, 102)]
    public int minExplosion;
    [Range(0, 102)]
    public int maxEplosion;
    [Range(0, 102)]
    public int minTired;
    [Range(0, 102)]
    public int maxTired;





    [Header("Bullet Attributes")]
    public GameObject BulletPivot;
    public GameObject TheBullet;
    public GameObject TheMuzzleEffect;
    GameObject instBullet;
    public float shootspeed;
    GameObject instmuzzle;

    [Header("Shooting Attributes")]
    public float myFloat2;
    public int tiempoEntreMens2 = 5; // the time that which we enter it , and iit does not matter if we change it





    [Header("Field Of View Attributes")]
    public float radius; // can see radius
    public float Nearradius; // near distance radius
    [Range(0, 360)]
    public float angle; // angle of vision
    public GameObject playerRef; // player refrence to take the player information
    public LayerMask targetMask; // player mask
    public LayerMask obstructionMask; // the obsticales mask
    public bool canSeePlayer;
    public bool Player_inside_near_radius;



    [Header("Health Attributes")]
    public float starthealth = 100; // the origin one
    public float health; // current health
    public Image healthbar; //for healthbar

    [Header("Shield Attributes")]
    public GameObject TheShield;
           GameObject instShield;
    public bool ShieldCreated;
    public GameObject ShieldPivot;


    [Header("Explosion Attributes")]
    public GameObject TheExplosion;
    GameObject instExplosion;
    public bool ExplosionCreated;
    public GameObject ExplosionPivot;





    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        aiState = AI_Distance_State.idle;
        nearState = Near_State_sitiuation.idle;
    }

    void Update()
    {
        
        if (StartEnemySystem)
        {
            if (canSeePlayer) // then he is in far area
            {
               

                if (dist <= Nearradius) // if near
                {

                    myFloat2 = 0;
                    aiState = AI_Distance_State.nearDistance;
                    transform.LookAt(player);



                }
                else
                {
                    aiState = AI_Distance_State.farDistance;
                    nearState = Near_State_sitiuation.idle;
                }

                if (aiState != AI_Distance_State.nearDistance || nearState != Near_State_sitiuation.shield) // shield conditions are correct
                {
                    ShieldCreated = false;
                    Destroy(instShield);
                }

                if (aiState != AI_Distance_State.nearDistance || nearState != Near_State_sitiuation.explosion)
                {
                    ExplosionCreated = false;
                    Destroy(instExplosion);
                }





            }
            else // can see player is ==false
            {
                aiState = AI_Distance_State.idle;
                nearState = Near_State_sitiuation.idle;


            }

            // new switch case depend on the field of the view
            switch (aiState)
            {
                case AI_Distance_State.farDistance:

                    CurrentState = "far Distance";
                    transform.LookAt(player);
                    Timer2();

                    break;
                case AI_Distance_State.nearDistance:
                    myFloat2 = 0;
                    CurrentState = "near Distance";
                    switch (nearState)
                    {
                        case Near_State_sitiuation.shield:
                            myFloat2 = 0;
                            CreateShield();

                            break;
                        case Near_State_sitiuation.explosion:
                            myFloat2 = 0;
                            CreateExplosion();
                            break;
                        case Near_State_sitiuation.tired:
                            myFloat2 = 0;
                            break;
                        default:
                            break;
                    }
                    myFloat2 = 0;
                    TimerRandomProbability();
                    // Timer(); the original random states
                    // we have to put switch case for every state
                    break;
                case AI_Distance_State.idle:
                    myFloat2 = 0; // the time for shooting the player will not shoot
                    break;
                default:
                    break;
            }
            // new switch case depend on the field of the view           
        }
    }
        

    public void Distance() // Extra function
    {
        dist = Vector3.Distance(player.position, transform.position);
    }
    
    public void Timer() // is timer to controll the shooting time for the enemy
    {
        // if we enter the area directly we will change from idle to the new state then after that we will change every specific amoiunt of secconds
        myFloat += Time.deltaTime;
        if (myFloat >= tiempoEntreMens || nearState == Near_State_sitiuation.idle)    // in each seccond we will check enter to see the condition
        {
                nearState = (Near_State_sitiuation)Random.Range(0, 3);
            counter++;
            if(nearState== Near_State_sitiuation.shield)
            {
                if (ShieldCreated == true)
                {

                }
            }

            if (nearState == Near_State_sitiuation.explosion)
            {
                if (ShieldCreated == true)
                {

                }
            }
            myFloat = 0; //we will reset it because the transcurrido here will count the secconds assummed
        }
    }


    public void shoot() 
    {
        instBullet = Instantiate(TheBullet, BulletPivot.transform.position, BulletPivot.transform.rotation, BulletPivot.transform) as GameObject;
        if (instBullet.GetComponent<Rigidbody>() == null)
        {
            instBullet.AddComponent<Rigidbody>();
            instBullet.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }
        instBullet.transform.position = BulletPivot.transform.position;

        instBullet.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * shootspeed, ForceMode.Impulse);
        instBullet.transform.SetParent(null);

        instmuzzle = Instantiate(TheMuzzleEffect, BulletPivot.transform.position, BulletPivot.transform.rotation, BulletPivot.transform) as GameObject;
        Destroy(instmuzzle, 2);
        Destroy(instBullet, 3);
    }


    public void Timer2() // is timer to controll the shooting time for the enemy
    {
            myFloat2 += Time.deltaTime;
            if (myFloat2 >= tiempoEntreMens2)    // in each seccond we will check enter to see the condition
            {
                transform.LookAt(player);
                shoot();
                myFloat2 = 0; //we will reset it because the transcurrido here will count the secconds assummed
            }
    }


    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true) // infinite loop in the coroutine as long is the condition is true , we will keep doing thi routine of seeing the player permenantly
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }


    private void FieldOfViewCheck() // is to check if we can see the player , from here we take the distance values
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask); // we will do overlapping sphere to detect if there is a player inside the sphere, depending on the mask value
        if (rangeChecks.Length != 0) // if the length != 0 which is mean there are objects inside the list of the collider we did inside the rangechecks , because we are storing them inside the list
        {
            Transform target = rangeChecks[0].transform; // we take the first one becuase usually there is only one player , only one object have this mask
            Vector3 directionToTarget = (target.position - transform.position).normalized; // to make the enemy take the direction to the player

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2) // depending on the angle vision we check the condition so the enemy can see the player or not , we divide the angle by 2 for more accuracy
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position); // we take the distance to use it in throwing ray from the enemy to the player
                //to set some values
                dist = distanceToTarget;
                //
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask)) // when we throw the ray we check if the hitting object is != this layer , if not so the ray is hitting the player , and we can see the player
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer) // we do this condition to reset it , in case the player was inside then he went outside the area
            canSeePlayer = false;
    }

    protected void takedamage(float amount) // if a character have a damage
    {
        health -= amount;
        healthbar.fillAmount = health / starthealth;
    }

    public void CreateShield()
    {
        if (ShieldCreated==false)
        {
            Debug.Log("Before Shield inistiated");
            instShield = Instantiate(TheShield, transform.position, transform.rotation, ShieldPivot.transform) as GameObject;
            Debug.Log("After Shield inistiated");
            ShieldCreated = true;
        }
    }

    public void CreateExplosion()
    {
        if (ExplosionCreated == false)
        {
            Debug.Log("Before Explosion inistiated");
            instExplosion = Instantiate(TheExplosion, transform.position, transform.rotation, ExplosionPivot.transform) as GameObject;
            Debug.Log("After Explosion inistiated");
            ExplosionCreated = true;
        }
    }


    public void RandomPosibilityState() // to do the decision in transition between the states of the near area states
    {
        digit = Random.Range(0, 102);
        if (digit >= minShield && digit <= maxShield)
        {
            nearState = Near_State_sitiuation.shield;
            CurrrentState = "1 is shield";
        }
        if (digit >= minExplosion && digit <= maxEplosion)
        {
            nearState = Near_State_sitiuation.explosion;
            CurrrentState = "2 is EXP";
        }
        if (digit >= minTired && digit <= maxTired)
        {
            nearState = Near_State_sitiuation.tired;
            CurrrentState = "3 is tired";
        }
    }


    public void TimerRandomProbability() // is timer to controll the shooting time for the enemy
    {
        // if we enter the area directly we will change from idle to the new state then after that we will change every specific amoiunt of secconds
        myFloat += Time.deltaTime;
        if (myFloat >= tiempoEntreMens || nearState == Near_State_sitiuation.idle)    // in each seccond we will check enter to see the condition
        {
            //  nearState = (Near_State_sitiuation)Random.Range(0, 3); // change this line by putting our random state function
            RandomPosibilityState();
            counter++;
            if (nearState == Near_State_sitiuation.shield)
            {
                if (ShieldCreated == true)
                {

                }
            }
            myFloat = 0; //we will reset it because the transcurrido here will count the secconds assummed
        }
    }

    public void StartEnemySystemBehaviour()
    {
        if (playerRef != null)
        {
            StartEnemySystem = true;
            StartCoroutine(FOVRoutine());
        }
        else
        {
            playerRef = GameObject.FindGameObjectWithTag("Player");
            StartEnemySystem = true;
            StartCoroutine(FOVRoutine());
        }
    }
}
