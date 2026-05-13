using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject _enemyPrefab;

    [Header("Spawn")]
    [SerializeField] private Transform[] _spawnPoints;

    [SerializeField] private int _maxEnemiesToSpawn = 20;
    [SerializeField] private float _spawnCooldown = 3f;

    private int _spawnedEnemies = 0;

    void Start()
    {
        InvokeRepeating(
            nameof(SpawnEnemy),
            0f,
            _spawnCooldown
        );
    }

    void SpawnEnemy()
    {
        // Para quando atingir o limite
        if (_spawnedEnemies >= _maxEnemiesToSpawn)
        {
            CancelInvoke(nameof(SpawnEnemy));
            return;
        }

        // Verifica se existem spawn points
        if (_spawnPoints.Length == 0)
        {
            Debug.LogWarning("Nenhum Spawn Point definido!");
            return;
        }

        // Escolhe posição aleatória
        Transform randomSpawn =
            _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        // Spawna inimigo
        Instantiate(
            _enemyPrefab,
            randomSpawn.position,
            randomSpawn.rotation
        );

        _spawnedEnemies++;
    }
}