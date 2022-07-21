using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public float ActiveTime = 2f; // the duration of the dash Trail ,when we click on the button

    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f; // the waiting time between every action , wehn we hold the button , so no lag or overloading happens
    public float meshDestroyDelay = 3f; // the time to destroy and remove the Effect from the Scene
    public Transform PositionTospawn; // to make the effect follow the character or the player


    private bool isTrailActive;

    private SkinnedMeshRenderer[] skinnedMeshRenderers; // the array is to take the componnents from the character children , like the skinned Mesh in every child of the parent

    [Header("Shader Related")]
    public Material mat;
    public string shaderVarRef;
    public float shaderVarRate ;
    public float shaderVarRefreshRater ;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)&& !isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(ActiveTrail(ActiveTime));
        }
    }

    IEnumerator ActiveTrail ( float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if (skinnedMeshRenderers == null)
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            for(int i =0; i<skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(PositionTospawn.position, PositionTospawn.rotation);
                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();
                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh); // to creat the Snapshoot of the mesh
                mf.mesh = mesh;
                mr.material = mat;
                StartCoroutine(AnimateMaterialFloat(mr.material , 0.1f , shaderVarRate , shaderVarRefreshRater));
                Destroy(gObj, meshDestroyDelay);
            }
            yield return new WaitForSeconds(meshRefreshRate);
        }
        isTrailActive = false;
    }

    IEnumerator AnimateMaterialFloat (Material mat , float goal , float rate , float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);
        while ( valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate);
        }
        yield return new WaitForSeconds(refreshRate);
    }
}
