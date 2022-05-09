using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamamPlayer : HamamCharacter
{
    protected override void Start()
    {
        base.Start();
        delmovemen = movementall;

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
}
