using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnController : MonoBehaviour
{
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] GameObject monsterPrefab;
    [SerializeField] float minTimeToNextSpawn;
    [SerializeField] float maxTimeToNextSpawn;
    [SerializeField] int maxAmountOfMonstersPerSpawn;
    bool spawning;
    void Start()
    {
        Play();
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            if (spawning)
            {
                int numberOfSpawns = Random.Range(1, maxAmountOfMonstersPerSpawn + 1);
                for(int i = 0; i < numberOfSpawns; i++)
                {
                    int spawnPointIndex = Random.Range(0, spawnPoints.Count);
                    Instantiate(monsterPrefab, spawnPoints[spawnPointIndex].transform.position, spawnPoints[spawnPointIndex].transform.rotation);
                }
                float timeTillNextSpawn = Random.Range(minTimeToNextSpawn, maxTimeToNextSpawn);
                yield return new WaitForSeconds(timeTillNextSpawn);
            }
            yield return null;
        }
    }

    public void Pause()
    {
        spawning = false;
    }

    public void Play()
    {
        spawning = true;
    }
}
