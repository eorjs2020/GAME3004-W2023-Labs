using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Target
{
    public Transform transform;
    public Vector3 offset;
    [Header("[If true, offset will be added to target]")]
    public bool x;
    public bool y;
}

//[ExecuteInEditMode]
public class MiniMapController : MonoBehaviour
{
    public Target target;
    

    


    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            (target.x) ? target.transform.position.x + target.offset.x : transform.position.x,
            transform.position.y,
            (target.y) ? target.transform.position.z + target.offset.z : transform.position.z
        );
    }
}
