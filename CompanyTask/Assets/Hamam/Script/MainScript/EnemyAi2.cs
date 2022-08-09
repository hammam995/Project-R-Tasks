using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.VFX;

public class EnemyAi2 : MonoBehaviour
{
    [Header("Start Enemy System")]
    public bool StartEnemySystem; // bool condition , to make the System start , we activate it from the Animation when it's finishing

    [Header("Player Attributes")]
    Transform player; // to take the refrence from the player
    public float dist; // to messure the distance between the Enemy and the Player
    public HamamPlayer HamamScript;


    public enum AI_Distance_State { nearDistance, farDistance, idle };
    public enum Near_State_sitiuation { idle, Circular , Circular2 };

    [Header("States Attributes")]
    public AI_Distance_State aiState = AI_Distance_State.farDistance;
    public Near_State_sitiuation nearState;
    [HideInInspector] public string CurrentState; // (near or far state) to show the current state , we can delete the variable, is usefull for testing


    [Header("Near States Attributes")]
    public int NearStatesIntervalTimer = 5; // The user can change it as much he want ,  tiempoEntreMens
    [HideInInspector] public int counter = 0; // (is usefull for testing , we can delete it) is counter just to check if we are moving between the states and to check when we do the transition , the sitituation of the objects we created , (shield, Explosion)
    [HideInInspector] public float NearStatesTransitionTimer; // it will change by the system ,  myfloat 



    [Header("Random Probability Near States Attributes")]
    public int digit; // to show the current Random number, depends on that we choose the near state sitiuation, because is connected to the percentage system
    [HideInInspector] public string CurrrentState; // is usefull to check from which state of the near state we are in
    [HideInInspector] public int minShield;
    [HideInInspector] public int maxShield;
    [HideInInspector] public int minExplosion;
    [HideInInspector] public int maxEplosion;
    [HideInInspector] public int minTired;
    [HideInInspector] public int maxTired;
    [Range(0, 100)]
    public int ShieldPercentage; // to put the percentage of the Shield state
    [Range(0, 100)]
    public int ExplosionPercentage; // to put the percentage of the Explosion state
    [Range(0, 100)]
    public int TiredPercentage; // to put the percentage of the Tired state


    [Header("Bullet Attributes")]
    public GameObject BulletPivot;
    public GameObject TheBullet;
    public GameObject TheMuzzleEffect;
    GameObject instBullet;
    GameObject instmuzzle;

    [Header("Shooting Attributes")]
    public int ShootingIntervalTime = 5; // The user can change it as much he want ,  tiempoEntreMens2
    public float shootspeed; // to control the bullet speed
    public float ShootTimer; // it will change by the system ,  myfloat2 

    [Header("(2) Shooting Attributes")]
    public int ShootingIntervalTime_2 = 3;
    public float ShootTimer_2; // it will change by the system ,  myfloat2 
    public bool Timer_2;





    [Header("Field Of View Attributes")]
    public float radius; // is the large Radius and area so we can see the player
    public float Nearradius; // near distance radius , is the small and near Radius area so we from it we do the transitions between the near states sitiuation
    [Range(0, 360)]
    public float angle; // angle of vision
    public GameObject playerRef; // player refrence to take the player information
    public LayerMask targetMask; // player mask
    public LayerMask obstructionMask; // the obsticales mask
    [HideInInspector] public bool canSeePlayer;


    [Header("Health Attributes")]
    public float starthealth = 100; // the origin one
    public float health; // current health
    public Image healthbar; //for healthbar

    [Header("Shield Attributes")]
    public GameObject TheShield;
    GameObject instShield;
    public GameObject ShieldPivot;
    [HideInInspector] public bool ShieldCreated;


    [Header("Explosion Attributes")]
    public GameObject TheExplosion;
    GameObject instExplosion;
    public GameObject ExplosionPivot;
    [HideInInspector] public bool ExplosionCreated;

    [Header(" Tired State Attributes")] // is to show the state VFX
    public GameObject TheTired;
    GameObject instTired;
    public GameObject StatePivot;
    [HideInInspector] public bool TiredCreated;

    [Header(" 360 State Attributes")] // is to show the state VFX
    [Range(0, 100)]
    public int CircularPercentage; // to put the percentage of the Shield state
    public float _speed;
    [HideInInspector] public int min360;
    [HideInInspector] public int max360;  
    [HideInInspector] public bool CircularCreated;
    [SerializeField] public Vector3 _rotation = Vector3.up;
    //public bool Look;

    [Header(" Laser_Beam Attributes")] // is to show the state VFX
    public GameObject laserPrefab;
    public GameObject firePoint;
    public float maximumLength;
    private LineRenderer lr;
    private GameObject spawnedLaser;


    [Header(" (2) 360 State Attributes")] // is to show the state VFX
    [Range(0, 100)]
    public int CircularPercentage_2; // to put the percentage of the Shield state
    public float _speed_2;
    [HideInInspector] public bool CircularCreated_2;
    [HideInInspector] public int min360_2;
    [HideInInspector] public int max360_2;
    [SerializeField] public Vector3 _rotation_2 = Vector3.up;


    [Header("(2) Laser_Beam Attributes")] // is to show the state VFX
    public float maximumLength_2;
    [Range(1f,2f)]
    public float Extent;
    private LineRenderer lr_2;
    private GameObject spawnedLaser_2;
    
    // we will use laser prefap
    // rather than fire poinr , use the Bullet Pivot

    public float Rotation_Torret;
    public GameObject Laser_Child;
    public Vector3 R1;
    public Vector3 R2;
    public float timeCount=0;

    public float t;
    public bool T;
    public float Rand;
    public float myFloat = 0; // the time will start from 0
    public int tiempoEntreMens = 1; // the time that which we enter it , and iit does not matter if we change it
    public int transcurrido = 0; // the counter or the pinter which is i

    private void Awake()
    {
        FindingPlayer(); // checked
    }

    void Start()
    {
        SettingStatesValue(); // checked
        settingThevalue(); // checked
        //
        spawnedLaser = Instantiate(laserPrefab, firePoint.transform) as GameObject;
        lr = spawnedLaser.transform.GetChild(0).GetComponent<LineRenderer>();
        DisableLaser();
        //GameObject Child = GameObjectsTransform.GetChild(The child index).gameObject

        //

        spawnedLaser_2 = Instantiate(laserPrefab, BulletPivot.transform) as GameObject;
        lr_2 = spawnedLaser_2.transform.GetChild(0).GetComponent<LineRenderer>();
        Laser_Child = spawnedLaser_2.transform.GetChild(0).gameObject;
        DisableLaser_2();
        StartCoroutine(RotationRoutine());

        //checked


        //  R1 = new Vector3(Laser_Child.transform.rotation.x, 0, Laser_Child.transform.rotation.z); ;
        //  R2 = new Vector3(Laser_Child.transform.rotation.x, Random.Range(-2f, 2f), Laser_Child.transform.rotation.z);
        //Laser_Child.transform.localEulerAngles = new Vector3(Laser_Child.transform.rotation.x, 0, Laser_Child.transform.rotation.z);
        // Laser_Child.transform.localEulerAngles = new Vector3(Laser_Child.transform.rotation.x, Random.Range(-2f, 2f), Laser_Child.transform.rotation.z);


    }

    void Update()
    {
        /* t += 0.1f * Time.deltaTime;
         if (t >= 1)
         {
             t = 0;
         }
         */
        /*  if (T == false)
          {
              t += t * Time.deltaTime;
              if (t > 1)
                  T = true;
          }
          else
          {
              t = 0;
              T = false;

          }

          */



        if (StartEnemySystem)
        {
            NearStatesActions();

            // new switch case depend on the field of the view
            switch (aiState)
            {
                case AI_Distance_State.farDistance:

                    CurrentState = "far Distance";
                     transform.LookAt(player);
                     myt();
                     Timer2();
                    // Checked far distance
                    EnableLaser_2();
                    break;
                case AI_Distance_State.nearDistance:
                    ShootTimer = 0;
                    CurrentState = "near Distance";
                    switch (nearState)
                    {

                        case Near_State_sitiuation.Circular:
                            ShootTimer = 0;
                            Create360();
                            EnableLaser();

                            break;

                        case Near_State_sitiuation.Circular2:



                            break;

                        default:
                            break;
                    }
                    ShootTimer = 0;
                    TimerRandomProbability();
                    // we have to put switch case for every state
                    break;
                case AI_Distance_State.idle:
                    ShootTimer = 0; // the time for shooting the player will not shoot
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
    

    public void Timer2() // is timer to controll the shooting time for the enemy , more or les is the fire rate
    {
        if (Timer_2 == false)
        {
            ShootTimer += Time.deltaTime;
            transform.LookAt(player);
            EnableLaser_2();
            if (ShootTimer >= ShootingIntervalTime)    // in each seccond we will check enter to see the condition
            {
                DisableLaser_2();
                ShootTimer_2 += Time.deltaTime;
                if (ShootTimer_2 >= ShootingIntervalTime_2)
                {
                   // EnableLaser_2();
                   // ShootTimer = 0;
                    Timer_2 = true;
                }
            }
        }
        else
        {
            if (Timer_2 == true)
            {
                ShootTimer = 0;
                ShootTimer_2 = 0;
                Timer_2 = false;
            }
        }
        
    }


    public void myt()
    {
        myFloat += Time.deltaTime;
        if (myFloat >= 0.3f)    // in each seccond we will check enter to see the condition
        {
            if (transcurrido >= tiempoEntreMens)
            {
                Rand = Random.Range(-2.5f, 2.5f);

                transcurrido = 0; //make the pointer 0 to reset it
                return; /// it will return to the first if
            }
            transcurrido++;
            myFloat = 0; //we will reset it because the transcurrido here will count the secconds assummed



        }
    }



    public IEnumerator RotationRoutine()
    {
        
        WaitForSeconds wait = new WaitForSeconds(0.3f); // the wait time to check from the vision system , every seccond we will do it how many times
       Laser_Child.transform.localEulerAngles = new Vector3(Laser_Child.transform.rotation.x, 0, Laser_Child.transform.rotation.z);

        
        while (true) // infinite loop in the coroutine as long is the condition is true , we will keep doing thi routine of seeing the player permenantly
        {

         //   Rand = Random.Range(-2.5f, 2.5f);

            R1 = new Vector3(Mathf.LerpAngle(Laser_Child.transform.rotation.x, Laser_Child.transform.rotation.x,  1f),
                              Mathf.LerpAngle(Laser_Child.transform.rotation.y, Rand, 1f),
                              Mathf.LerpAngle(Laser_Child.transform.rotation.z, Laser_Child.transform.rotation.z, 1f)
                );


             Laser_Child.transform.localEulerAngles = R1;


            yield return wait;
            // Laser_Child.transform.localEulerAngles = new Vector3(Laser_Child.transform.rotation.x, Random.Range(-2f, 2f), Laser_Child.transform.rotation.z);
            //    R1 = new Vector3(Laser_Child.transform.rotation.x, Laser_Child.transform.rotation.y, Laser_Child.transform.rotation.z); ;
            //    R2 = new Vector3(Laser_Child.transform.rotation.x, Random.Range(-2f, 2f), Laser_Child.transform.rotation.z);
            //  transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, timeCount * speed);
            //  timeCount = timeCount + Time.deltaTime;
        }
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f); // the wait time to check from the vision system , every seccond we will do it how many times
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

    public void CreateShield() // to inistiate and create the shield in the shield object inside the enemy
    {
        if (ShieldCreated == false)
        {
            instShield = Instantiate(TheShield, transform.position, transform.rotation, ShieldPivot.transform) as GameObject;
            ShieldCreated = true;
        }
    }

    public void CreateExplosion() // to inistiate and create the Explosion in the Explosion object inside the enemy
    {
        if (ExplosionCreated == false)
        {
            instExplosion = Instantiate(TheExplosion, transform.position, transform.rotation, ExplosionPivot.transform) as GameObject;
            ExplosionCreated = true;
        }
    }

    public void CreateTired()
    {
        if (TiredCreated == false)
        {
            instTired = Instantiate(TheTired, transform.position, transform.rotation, StatePivot.transform) as GameObject;
            TiredCreated = true;
        }
    }

    public void Create360()
    {

        transform.Rotate(_rotation * _speed * Time.deltaTime);

    }


    public void RandomPosibilityState() // to do the decision in transition between the states of the near area states , The System and the behavior of Random States with Percentage , inner calculations
    {
        digit = Random.Range(0, 102);

        if (digit >= min360 && digit <= max360)
        {
            nearState = Near_State_sitiuation.Circular;
            CurrrentState = "4 is 360";
        }

        if (digit >= min360_2 && digit <= max360_2)
        {
            nearState = Near_State_sitiuation.Circular2;
            CurrrentState = "Cicular2";
        }
    }

    public void TimerRandomProbability() // is timer to controll the trasition time between the states in the near states sitiuations
    {
        NearStatesTransitionTimer += Time.deltaTime;
        if (NearStatesTransitionTimer >= NearStatesIntervalTimer || nearState == Near_State_sitiuation.idle)
        {
            RandomPosibilityState();
            counter++;
            NearStatesTransitionTimer = 0; //we will reset it because the transcurrido here will count the secconds assummed
        }
    }

    public void StartEnemySystemBehaviour() // to start the enemy behavior , the system will start when the animation of going up finish , inside the animation the function will be activated from there  ,we will start the behaviour , wehn the enemy animation finish when the enemy machine go up , then it will start it's behaviour
    {
        if (playerRef != null) // we check if it's not null , to avoid errors , in case we didn't put the object , or in case we didn't set it in the start
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
    public void settingThevalue() // to set the system of the % percentage , the total of all the percentage must not increase than 100% , to avoid errors in calculating , weput it in the start , because the user will set the value of the states before we start the game
    {
        min360 = 0;
        max360 = CircularPercentage;
        min360_2 = max360 + 1;
        max360_2 = min360_2 + CircularPercentage_2;
    }

    public void FindingPlayer() // we put this in the Awake or the start there is no much diffrent
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void SettingStatesValue() // this is in the start , to set the values of the states at the beginning , to set the variables to the default values
    {
        aiState = AI_Distance_State.idle;
        nearState = Near_State_sitiuation.idle;
    }

    public void NearStatesActions()
    {

        if (canSeePlayer) // then he is in far area
        {
            if (dist <= Nearradius) // if near
            {
                ShootTimer = 0;
                DisableLaser_2();
                aiState = AI_Distance_State.nearDistance;
                //transform.LookAt(player);
            }
            else
            {
                aiState = AI_Distance_State.farDistance;
                nearState = Near_State_sitiuation.idle;
            }
            
            if (aiState != AI_Distance_State.nearDistance || nearState != Near_State_sitiuation.Circular)
            {
                CircularCreated = false;
                DisableLaser();
                // Destroy(instTired);
            }
           /* if (aiState != AI_Distance_State.nearDistance)
            {
                DisableLaser_2();
            }*/



        }

        else // can see player is ==false
        {
            SettingStatesValue();
        }
    }
   
    public void EnableLaser()
    {
        spawnedLaser.SetActive(true);

    }
    public void DisableLaser()
    {
        spawnedLaser.SetActive(false);

    }

    public void Laser_L()
    {
        lr.SetPosition(1, new Vector3(0, 0, maximumLength));
    }

    ////////////////////////
    ///
    public void EnableLaser_2()
    {
        Laser_L_2();
        spawnedLaser_2.SetActive(true);
       // Laser_Child.transform.localEulerAngles = new Vector3(Laser_Child.transform.rotation.x, Random.Range(-2, 2), Laser_Child.transform.rotation.z);

    }
    public void DisableLaser_2()
    {

        spawnedLaser_2.SetActive(false);
       // Laser_Child.transform.localEulerAngles = new Vector3(Laser_Child.transform.rotation.x, 0, Laser_Child.transform.rotation.z);
       // Laser_Child.transform.localEulerAngles = new Vector3(Laser_Child.transform.rotation.x, Random.Range(-2f, 2f), Laser_Child.transform.rotation.z);
    }
    public void Laser_L_2()
    {
        lr_2.SetPosition(1, new Vector3(0, 0, dist * 1.5f));
    }
}
