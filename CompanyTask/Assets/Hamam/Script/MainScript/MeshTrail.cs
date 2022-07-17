using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public float ActiveTime = 2f;

    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 3f;
    public Transform PositionTospawn;


    private bool isTrailActive;

    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    [Header("Shader Related")]
    public Material mat;




    void Start()
    {
        
    }

    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.RightShift)&& !isTrailActive)
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
                Destroy(gObj, meshDestroyDelay);
            }
            yield return new WaitForSeconds(meshRefreshRate);
        }
        isTrailActive = false;
    }
}
