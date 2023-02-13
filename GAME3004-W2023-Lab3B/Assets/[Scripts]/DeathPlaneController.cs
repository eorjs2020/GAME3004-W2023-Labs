using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DeathPlaneController : MonoBehaviour
{

   

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            other.GetComponent<PlayerBehaviour>()?.Respawn();            
        }
    }    
}
