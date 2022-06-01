using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlash : MonoBehaviour
{
    public float speed = 30; // to controll hoe fast it's and the Distance , because we are controlling it from the RigidBody
    public float slowDownRate = 0.01f;  // it's prefered to be it's range between [0.01 -1]
    public float destroyDelay = 5; // the time to destroy the slash

    private Rigidbody rb;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if (GetComponent<Rigidbody>() != null)
        {
            rb = GetComponent<Rigidbody>();
            StartCoroutine(SlowDown());
        }
        else
            Debug.Log("No Rigidbody");

        Destroy(gameObject, destroyDelay);
    }
    IEnumerator SlowDown ()
    {
        float t = 1;
        while (t > 0)
        {
            rb.velocity = Vector3.Lerp(Vector3.zero, rb.velocity, t);
            t -= slowDownRate;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
