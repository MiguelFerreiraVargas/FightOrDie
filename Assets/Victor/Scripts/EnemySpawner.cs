using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Spawn Config")]
    [SerializeField] private int maxTotalEnemies = 20;
    [SerializeField] private int maxEnemiesAlive = 4;
    [SerializeField] private float spawnCooldown = 3f;

    // Controle
    private int totalSpawnedEnemies = 0;

    // Lista de inimigos vivos
    private readonly List<GameObject> aliveEnemies =
        new List<GameObject>();

    void Start()
    {
        InvokeRepeating(
            nameof(SpawnEnemy),
            0f,
            spawnCooldown
        );
    }

    void SpawnEnemy()
    {
        // Limite total
        if (totalSpawnedEnemies >= maxTotalEnemies)
        {
            CancelInvoke(nameof(SpawnEnemy));
            return;
        }

        // Limite vivos
        CleanEnemyList();

        if (aliveEnemies.Count >= maxEnemiesAlive)
            return;

        // Sem spawn points
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning(
                "EnemySpawner: nenhum Spawn Point definido."
            );

            return;
        }

        // Procura spawn livre
        List<Transform> freeSpawnPoints =
            GetFreeSpawnPoints();

        // Nenhum livre
        if (freeSpawnPoints.Count == 0)
            return;

        // Escolhe spawn aleatório
        Transform randomSpawn =
            freeSpawnPoints[
                Random.Range(0, freeSpawnPoints.Count)
            ];

        // Cria inimigo
        GameObject enemy = Instantiate(
            enemyPrefab,
            randomSpawn.position,
            randomSpawn.rotation
        );

        // Adiciona na lista
        aliveEnemies.Add(enemy);

        totalSpawnedEnemies++;
    }

    private List<Transform> GetFreeSpawnPoints()
    {
        List<Transform> freePoints =
            new List<Transform>();

        foreach (Transform spawnPoint in spawnPoints)
        {
            bool occupied = false;

            foreach (GameObject enemy in aliveEnemies)
            {
                if (enemy == null)
                    continue;

                float distance =
                    Vector3.Distance(
                        enemy.transform.position,
                        spawnPoint.position
                    );

                // Se tiver inimigo muito perto
                if (distance < 2f)
                {
                    occupied = true;
                    break;
                }
            }

            if (!occupied)
                freePoints.Add(spawnPoint);
        }

        return freePoints;
    }

    // Remove inimigos destruídos da lista
    private void CleanEnemyList()
    {
        aliveEnemies.RemoveAll(enemy => enemy == null);
    }
}