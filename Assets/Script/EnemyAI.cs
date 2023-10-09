using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    private Transform myTransform;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling tuần tra
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;


    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    [SerializeField] private GameObject projecttile;

    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
        myTransform = transform;
    }

    private void Update()
    {
        //check for sight and attack range
        playerInSightRange = Physics.CheckSphere(myTransform.position, sightRange, whatIsPlayer);//trong phạm vi đuổi theo
        playerInAttackRange = Physics.CheckSphere(myTransform.position,attackRange, whatIsPlayer);//trong phạm vi tấn công

        if(!playerInSightRange && !playerInAttackRange)Patroling();
        if(playerInSightRange && !playerInAttackRange) ChasePlayer();
        if(!playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        //tuần tra
        if (!walkPointSet) SearchWalkPoint();

        if(walkPointSet)agent.SetDestination(walkPoint);

        Vector3 distanceToWalkpoint = myTransform.position - walkPoint;

        if(distanceToWalkpoint.magnitude  <1f)
            walkPointSet = false;
       
    }

    private void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(myTransform.position.x + randomX, myTransform.position.y,myTransform.position.z +randomZ);
        if(Physics.Raycast(walkPoint,-myTransform.up,2f,whatIsGround)) 
        { 
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        //đuổi theo player
        agent.SetDestination(player.position);
    }    

    private void AttackPlayer()
    {
        agent.SetDestination(myTransform.position);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            //attack code here
            Rigidbody rb = Instantiate(projecttile,myTransform.position,Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(myTransform.forward *32f,ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }    

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }    
}
