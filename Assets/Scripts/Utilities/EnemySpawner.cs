using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] EnemyData enemyData;
    [SerializeField] List<Transform> spawnPoints;


    private void Start()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        foreach (Transform point in spawnPoints)
        {
            EnemyFactory.CreateEnemy(enemyPrefab, enemyData, point.position, enemyPrefab.transform.rotation);
        }
    }

}
