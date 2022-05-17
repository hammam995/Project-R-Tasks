using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform CameraTargret;
    [SerializeField] float sSpeed;
    [SerializeField] Vector3 dist;
    [SerializeField] Transform LoockTarget;
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        Vector3 dPos = CameraTargret.position + dist;
        Vector3 sPos = Vector3.Lerp(transform.position, dPos, sSpeed * Time.deltaTime);
        transform.position = sPos;
        transform.LookAt(LoockTarget);
    }
    void Update()
    {



    }
}
