using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinScript : MonoBehaviour
{

    public float startYPosition;
    
    void Update()
    {
        
        transform.Rotate(0f, 10 * Time.deltaTime, 0f, Space.Self);           
        transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time, 1f) + startYPosition, transform.position.z);
    }
}
