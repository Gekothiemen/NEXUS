using UnityEngine;
using UnityEngine.AI;

public class AI_Target : MonoBehaviour
{
    public Transform playerPosition;
    public float attackDistance;
    private NavMeshAgent enemyAgent;
    private float playerDistance;
    public PlayerHealth playerhealth;
    public GameObject player;
    bool noticesPlayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    private void OnTriggerStay(Collider player)
    {
        playerDistance = Vector3.Distance(enemyAgent.transform.position, playerPosition.position);
        if (playerDistance < attackDistance & player != null)
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
        playerhealth.OnTakeDamage(10);
    }
}
