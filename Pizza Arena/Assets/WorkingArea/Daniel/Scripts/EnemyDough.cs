using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyDough : Enemy
{
    [SerializeField] List<Transform> players;
    [SerializeField] float minDistanceToPlayer;
    [SerializeField] float targetChangePeriod;
    NavMeshAgent agent;
    Transform player;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // TODO get players from Game manager instead of getting them from the serialized field
    }

    public void Start()
    {
        StartCoroutine(CheckForClosestPlayer());
    }
    private void Update()
    {
        if (GetProyectedDistance(player.position, transform.position) > minDistanceToPlayer)
        {
            if (agent.isStopped)
                agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
        }
    }
    public override void TakeDamage()
    {
        throw new System.NotImplementedException();
    }

    private void SelectPlayerToFollow()
    {
        float minDistance = 1000000;
        foreach(Transform thisPlayer in players)
        {
            float thisDistance = GetProyectedDistance(thisPlayer.position, transform.position);
            if (thisDistance < minDistance)
            {
                player = thisPlayer;
                minDistance = thisDistance;
            }
        }
    }

    private Vector3 GetProyectedVector(Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);
    }
    
    private float GetProyectedDistance(Vector3 vec1, Vector3 vec2)
    {
        vec1 = GetProyectedVector(vec1);
        vec2 = GetProyectedVector(vec2);
        return Vector3.Distance(vec1, vec2);
    }

    IEnumerator CheckForClosestPlayer()
    {
        while (true){
            SelectPlayerToFollow();
            yield return new WaitForSeconds(targetChangePeriod);
        }
    }
}
