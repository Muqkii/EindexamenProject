using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Patroling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public bool playerInSight, playerInRange;

    private EnemySight enemySight;

    private void Awake()
    {
        player = GameObject.Find("New Player").transform;
        agent = GetComponent<NavMeshAgent>();
        enemySight = GetComponent<EnemySight>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemySight.isSpotted && !enemySight.isInFirerange)
        {
            Patroling();
        }
        if (enemySight.isSpotted && !enemySight.isInFirerange)
        {
            ChasePlayer();
        }
        if (enemySight.isSpotted && enemySight.isInFirerange)
        {
            StopToAttack();
        }
    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void StopToAttack()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);
    }
}
