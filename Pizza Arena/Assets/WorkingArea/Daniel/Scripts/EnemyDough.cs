using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyDough : Enemy, Damageable
{
    [SerializeField] List<Transform> players;
    [SerializeField] float minDistanceToPlayer;
    [SerializeField] float targetChangePeriod;
    [SerializeField] float attackAreaDimensions;
    [SerializeField] float attackDuration;
    [SerializeField] int damageDealt;
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
        StartCoroutine(AttackingRoutine());
    }
    private void Update()
    {
        float distance = GetProyectedDistance(player.position, transform.position);
        
        if (distance > minDistanceToPlayer)
        {
            if (agent.isStopped)
                agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }
    }

    //Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + transform.forward * minDistanceToPlayer, new Vector3(attackAreaDimensions, attackAreaDimensions, attackAreaDimensions));
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

    void TryDamagingPlayers()
    {
        RaycastHit[] hits;
        hits = Physics.BoxCastAll(transform.position + transform.forward * minDistanceToPlayer, new Vector3(attackAreaDimensions, attackAreaDimensions, attackAreaDimensions), transform.forward);
        foreach(RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Damageable player = hit.collider.gameObject.GetComponent<Damageable>();
                if (player != null)
                {
                    player.TakeDamage(damageDealt);
                }
                else
                {
                    Debug.LogError(hit.collider.gameObject.name + " doesn't have a Damageable component, add one or remove the Player tag");
                }
            }
        }
    }

    public void TakeDamage(int damageAmmount)
    {
        throw new System.NotImplementedException();
    }

    IEnumerator AttackingRoutine()
    {
        while (true)
        {
            if (agent.isStopped)
            {
                TryDamagingPlayers();
                yield return new WaitForSeconds(attackDuration);
            }
            yield return null;
        }
    }
}
