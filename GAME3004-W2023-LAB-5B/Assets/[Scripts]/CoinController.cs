using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private float startYposition;

    // Start is called before the first frame update
    void Start()
    {
        startYposition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x,
            Mathf.PingPong(Time.time, 0.5f) + startYposition, transform.position.z);
        transform.RotateAround(transform.position, Vector3.up, 50.0f * Time.deltaTime);
    }
}
