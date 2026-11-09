using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class AI_Enemy : MonoBehaviour
{
    public Transform playerPosition;
    private float attackDistance;
    private NavMeshAgent enemyAgent;
    private float playerDistance;
    public PlayerHealth playerhealth;
    public GameObject player;
    bool noticesPlayer;
    bool onCoolDown;
    public Vector3 minPos;
    public Vector3 maxPos;

    FieldOfView fieldofview;
    private bool isInRadius;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();

       
    }

    // Update is called once per frame
    void Update()
    {
        
            
            InEnemyRadius();
        
    }

    public void InEnemyRadius()
    {
        playerDistance = Vector3.Distance(enemyAgent.transform.position, playerPosition.position);
        if (playerDistance < attackDistance & fieldofview.canSeePlayer == true)
        {
            enemyAgent.isStopped = true;
            attackPlayer();
        }
        else
        {
            enemyAgent.isStopped = false;
            enemyAgent.destination = playerPosition.position;
        }
    }
 

    public void attackPlayer()
    {
        if (onCoolDown == false)
        {
            playerhealth.OnTakeDamage(10);
        }

    }
}
