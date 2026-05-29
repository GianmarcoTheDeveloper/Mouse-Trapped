using UnityEngine;

public static class EnemyFactory
{

    public static GameObject CreateEnemy(GameObject enemyPrefab,EnemyData enemyStats,Vector3 position, Quaternion rotation)
    {

        GameObject enemyInstance = Object.Instantiate(enemyPrefab,position,rotation);

        if (enemyInstance.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
        {
            enemy.Initialize(enemyStats);
        }
        else
        {
            Debug.Log("Prefab does not contain an enemy script");
            return null;
        }

            return enemyInstance;
    }
}
