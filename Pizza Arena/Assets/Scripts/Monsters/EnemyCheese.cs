using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyCheese : Enemy, Damageable
{
    [SerializeField] float minDistanceToPlayer;
    [SerializeField] float targetChangePeriod;
    [SerializeField] float attackAreaDimensions;
    [SerializeField] float attackDuration;
    [SerializeField] float attackCoolDown;
    [SerializeField] int damageDealt;
    [SerializeField] float perceptionDistance;
    [SerializeField] GameObject spawningItem;
    [SerializeField] int minAmmountItems;
    [SerializeField] int maxAmmountItems;
    [SerializeField] MonsterData data;
    NavMeshAgent agent;
    Transform player = null;
    Vector3 movePositionWhileSearching;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Start()
    {
        movePositionWhileSearching = transform.position;
        base.Start();
        StartCoroutine(AttackingRoutine());
        StartCoroutine(SearchAndDestroy());
    }

    //Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + transform.forward * minDistanceToPlayer + transform.up * attackAreaDimensions / 2, new Vector3(attackAreaDimensions, attackAreaDimensions, attackAreaDimensions));
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, perceptionDistance);
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

    public Vector3 RandomNavmeshLocation(float radius)
    {
        bool foundValidPosition = false;
        Vector3 finalPosition = Vector3.zero;
        while (!foundValidPosition)
        {
            Vector3 randomDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized * radius;
            randomDirection += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
                foundValidPosition = true;
            }
        }
        return finalPosition;
    }

    void TryDamagingPlayers()
    {
        RaycastHit[] hits;
        hits = Physics.BoxCastAll(transform.position + transform.forward * minDistanceToPlayer + transform.up * attackAreaDimensions/2, new Vector3(attackAreaDimensions, attackAreaDimensions, attackAreaDimensions), transform.forward, Quaternion.identity, 0);
        foreach(RaycastHit hit in hits)
        { 
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Damageable player = hit.collider.gameObject.GetComponent<Damageable>();
                if (player != null)
                {
                    int newHealt = player.TakeDamage(damageDealt);
                    if (newHealt <= 0)
                        this.player = null;
                }
                else
                {
                    Debug.LogError(hit.collider.gameObject.name + " doesn't have a Damageable component, add one or remove the Player tag");
                }
            }
        }
    }

    public int TakeDamage(int damageAmount)
    {
        data.RemoveHealth(damageAmount);
        if(data.GetHealth() <= 0 && GetState() != State.DYING)
        {
            StartCoroutine(Despawn());
        }
        return data.GetHealth();
    }

    IEnumerator AttackingRoutine()
    {
        while (GetState() != State.DYING)
        {
            if (agent.isStopped)
            {
                NotifyObservers(State.ATTACKINGMELEE);
                TryDamagingPlayers();
                yield return new WaitForSeconds(attackDuration);
                if (GetState() == State.DYING || GetState() == State.SEARCHINGPLAYER)
                    yield break;
                
                NotifyObservers(State.IDLE);
                yield return new WaitForSeconds(attackCoolDown);
                
            }
            yield return null;
        }
    }

    IEnumerator Despawn()
    {
        NotifyObservers(State.DYING);
        yield return new WaitForSeconds(2);
        int spawnedItemsNumber = Random.Range(minAmmountItems, maxAmmountItems + 1);
        for(int i = 0; i < spawnedItemsNumber; i++)
        {
            GameObject newItem = Instantiate(spawningItem,transform.position + new Vector3(Random.Range(0.0f, 1.0f), 0, Random.Range(0.0f, 1.0f)), Quaternion.identity);
        }
        Destroy(gameObject);
    }

    IEnumerator SearchAndDestroy()
    {
        bool hadPlayerLastFrame = false;
        NotifyObservers(State.SEARCHINGPLAYER);
        while(GetState() != State.DYING)
        {
            if(player == null)
            {
                // try spotting a player inside the perception sphere
                Collider[] hits = Physics.OverlapSphere(transform.position, perceptionDistance);
                foreach(Collider collider in hits)
                {
                    if (collider.gameObject.CompareTag("Player"))
                    {
                        //NotifyObservers(State.WALKINGTOWARDSPLAYER);
                        player = collider.gameObject.transform;
                        break;
                    }
                }

                if(player == null && GetProyectedDistance(transform.position, movePositionWhileSearching) < minDistanceToPlayer)
                {
                    // if player is still null the enemy has reached its destination generate new random destination
                    movePositionWhileSearching = RandomNavmeshLocation(perceptionDistance);
                    agent.SetDestination(movePositionWhileSearching);
                }
            }
            else
            {
                // we found a target, chase it
                float distance = GetProyectedDistance(player.position, transform.position);
                // if we still need to chase the player
                if (distance > perceptionDistance * 1.1f)
                {
                    // if the player is too far away remove it and start searching again
                    agent.isStopped = false;
                    player = null;
                    movePositionWhileSearching = RandomNavmeshLocation(perceptionDistance);
                    agent.SetDestination(movePositionWhileSearching);
                }
                else
                {
                    if (distance > minDistanceToPlayer)
                    {
                        // if the player is still reachable follow the player
                        if (agent.isStopped)
                        {
                            agent.isStopped = false;
                        }
                        agent.SetDestination(player.position);
                    }
                    else
                    {
                        // it we reached them, stop moving
                        agent.velocity = Vector3.zero;
                        agent.isStopped = true;
                    }
                }
            }
            if(player == null && hadPlayerLastFrame)
            {
                // this means the player was lost in this frame, start searching
                NotifyObservers(State.SEARCHINGPLAYER);
            }
            if(player != null && !hadPlayerLastFrame)
            {
                // this means a player was found this frame, start chasing
                NotifyObservers(State.WALKINGTOWARDSPLAYER);
            }
            hadPlayerLastFrame = player != null;
            yield return null;
        }
    }
}
