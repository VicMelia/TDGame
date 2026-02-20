using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] rewards;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxSpawns = 5;
    [SerializeField] private int enemiesDefeated = 0;
    [SerializeField] private float spawnInterval = 5f;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
    }

    public void OnEnemyDefeated()
    {
        if(++enemiesDefeated >= maxSpawns) 
        {
            for (int i = 0; i < rewards.Length; i++)
            {
                Instantiate(rewards[i], spawnPoints[i].position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        int spawnCount = 0;
        while (spawnCount < maxSpawns)
        {
            int index = Random.Range(0, spawnPoints.Length);
            EnemyCharacter enemyCharacter = Instantiate(enemyPrefab, spawnPoints[index].position, Quaternion.identity).GetComponent<EnemyCharacter>();
            enemyCharacter.SetSpawner(this);
            spawnCount++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
