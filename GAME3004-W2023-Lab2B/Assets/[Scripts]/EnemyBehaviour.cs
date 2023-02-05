using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;
public class EnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent Agent;
    private CharacterController Controller;
    public GameObject Player;

    public float MovementSpeed = 4.0f;
    public float EnemyRestRange = 5f;

   

    private float verticalVelocity;

    public Vector3 Movement => Vector3.up * verticalVelocity;
    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>(); 
        Controller = GetComponent<CharacterController>();
        Player = FindObjectOfType<PlayerBehaviour>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (verticalVelocity < 0f && Controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        if (!IsEnemyInProperPosition())
        {
            MoveToPlayer(Time.deltaTime);
            FacePlayer();
        }


    }

    private void MoveToPlayer(float deltaTime)
    {
        if (Player == null) { return; }
        if (Agent.isOnNavMesh)
        {
            Agent.destination = Player.transform.position;

            Move(Agent.desiredVelocity.normalized * MovementSpeed, deltaTime);
        }

        Agent.velocity = Controller.velocity;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        Controller.Move((motion + Movement) * deltaTime);
    }
    private bool IsEnemyInProperPosition()
    {
        if (Player == null) { return false; }

        float playerDistanceSqr = (Player.transform.position - transform.position).sqrMagnitude;

        return playerDistanceSqr <= EnemyRestRange * EnemyRestRange;
    }

    protected void FacePlayer()
    {
        if (Player == null) { return; }

        Vector3 lookPos = Player.transform.position - transform.position;
        lookPos.y = 0.0f;

        transform.rotation = Quaternion.LookRotation(lookPos);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnemyRestRange);      
    }
}
