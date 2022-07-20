using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HamamPlayer : HamamCharacter
{

    [Header("Ground Slash System")]
    public GameObject projectile;
    public Transform firePoint;
    public float fireRate = 4;

    private float timeToFire;
    private GroundSlash groundSlashScript;

    private Animator anim;


    protected override void Start()
    {
        base.Start();
        delmovemen = movementall;
        delmovemen += isWalking;
        delmovemen += isRunning;
        starthealth = 100;
        health = starthealth;

        anim = GetComponent<Animator>();

    }

    public void isWalking() // the condition to activate the animation for walking and turnningaround by using everyoune it is input axis
    {
        anim.SetFloat("Walk", Input.GetAxis("Vertical"));
    }

    public void isRunning() // the condition activate the running animation
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") != 0 || Input.GetKey(KeyCode.RightShift) && Input.GetAxis("Vertical") != 0)
        {
            anim.SetFloat("Walk", 2 * Input.GetAxis("Vertical"));
        }
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
        if (Input.GetKey(KeyCode.X) && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / fireRate;
            ShootProjectile();
        }
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
        }
    }

    void ShootProjectile()
    { 
            InstantiateProjectileAtFirePoint();   
    }

    void InstantiateProjectileAtFirePoint()
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity) as GameObject;

        groundSlashScript = projectileObj.GetComponent<GroundSlash>();
        RotateToDestination(projectileObj, firePoint.transform.forward * 1000, true);
        projectileObj.GetComponent<Rigidbody>().velocity = firePoint.transform.forward * groundSlashScript.speed;
    }

    void RotateToDestination(GameObject obj, Vector3 destination, bool onlyY)
    {
        var direction = destination - obj.transform.position;
        var rotation = Quaternion.LookRotation(direction);

        if (onlyY)
        {
            rotation.x = 0;
            rotation.z = 0;
        }
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }
}
