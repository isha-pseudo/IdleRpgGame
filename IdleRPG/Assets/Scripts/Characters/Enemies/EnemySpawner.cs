using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Vector3[] spawnPoints = new Vector3[1];
    [SerializeField] private EnemyDataSO defaultEnemyData;

    private List<SpawnedEnemy> spawnedEnemies = new List<SpawnedEnemy>();

    [System.Serializable]
    private class SpawnedEnemy
    {
        public GameObject instance;
        public EnemyDataSO rememberedData;
        public float respawnTimer;
        public Vector3 spawnPoint;
    }
    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            spawnPoints = new Vector3[] { new Vector3 (0, 0, 10), new Vector3(5, 0, 10), new Vector3(-5, 0, 10)};
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnEnemy(spawnPoints[i], defaultEnemyData);
        }
    }

    private void SpawnEnemy(Vector3 position, EnemyDataSO data)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        EnemyHandler handler = newEnemy.GetComponent<EnemyHandler>();
        if (handler != null)
        {
            handler.enemyData = data;
        }

        spawnedEnemies.Add(new SpawnedEnemy{
            instance = newEnemy,
            rememberedData = data,
            respawnTimer = 0f,
            spawnPoint = position
        });
    }

    void Update()
    {
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            var enemy = spawnedEnemies[i];
            if (!enemy.instance.activeSelf && enemy.respawnTimer <= 0f)
            {
                enemy.respawnTimer = enemy.rememberedData.respawnDelay;
            }
            else if (enemy.respawnTimer > 0f)
            {
                enemy.respawnTimer -= Time.deltaTime;
                if (enemy.respawnTimer <= 0f)
                {
                    enemy.instance.transform.position = enemy.spawnPoint;
                    EnemyHandler handler = enemy.instance.GetComponent<EnemyHandler>();
                    if (handler != null) handler.ResetEnemy();
                    enemy.instance.SetActive(true);
                }
            }
        }
    }
}
