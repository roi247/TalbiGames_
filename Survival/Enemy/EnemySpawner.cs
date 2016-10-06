using UnityEngine;
using System.Collections;

namespace SurvivalGame
{

    public class EnemySpawner : MonoBehaviour
    {
        public EnemySpawnerData enemySpawnerData;
        [SerializeField]
        Enemy enemyPrefab;
        [SerializeField]
        SurvivalPlayer playerPrefab;


        // Use this for initialization
        void Start()
        {
            StartCoroutine(SpawnEnemiesRoutine());
        }

        IEnumerator SpawnEnemiesRoutine()
        {
            int enemyCount = 0;
            yield return new WaitForSeconds(enemySpawnerData.spawnDelay);
            while (true)
            {
                if (enemyCount < enemySpawnerData.maxEnemiesAtTheMoment)
                {
                    Enemy enemy = Instantiate(enemyPrefab, enemySpawnerData.GetNextSpawnPoint().position, Quaternion.identity) as Enemy;
                    enemy.SetTarget(playerPrefab);
                    enemyCount++;
                }
                yield return new WaitForSeconds(enemySpawnerData.spawnDelay);
            }

        }




    }

    [System.Serializable]
    public class EnemySpawnerData
    {
        public Transform[] spawnPoints;
        public float spawnDelay;
        public int maxEnemiesAtTheMoment;

        public Transform GetNextSpawnPoint()
        {
            int _rand = Random.Range(0, spawnPoints.Length);
            return (spawnPoints[_rand]);
        }



    }
}
