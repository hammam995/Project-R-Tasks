using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HamamCharacter : MonoBehaviour
{
    public float runSpeed =100; // speed for movement and rotation
    public delegate void delegadem();
    public delegadem delmovemen; //delegade variable for movement and their animation
    public float walkingSpeed = 5;
    public float RotateSpeed = 10;


    [Header("Health Attributes")]
    public float starthealth; // the origin one
    public float health; // current health
    public Image healthbar; //for healthbar



    protected virtual void Start()
    {
        
    }

    

    protected virtual void movementall() // method to walk in all destination
    {
         if ((Input.GetKey(KeyCode.W)) && Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.UpArrow)) && Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.UpArrow)))
        {
            transform.Translate(Vector3.forward * walkingSpeed * Time.deltaTime);
        }

        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(-1 * Vector3.forward * runSpeed * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate( -1 * Vector3.forward * walkingSpeed * Time.deltaTime);
        }
       
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -8 * RotateSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 8 * RotateSpeed * Time.deltaTime, 0);
        }

    }

    protected virtual void rotation(float direction) // turning around
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
