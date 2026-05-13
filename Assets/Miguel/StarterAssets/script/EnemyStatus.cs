using UnityEngine;

public class EnemyStatus : MonoBehaviour, IShootable
{
    [Header("Vida")]
    [SerializeField] private float lifeMax = 2f;
    private float currentLife;

    [Header("Movimento")]
    [SerializeField] private float followDistance = 10f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Ataque")]
    [SerializeField] private float knockbackForce = 7f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Efeitos")]
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private float bloodSize = 2f;

    [Header("UI")]
    [SerializeField] private GameObject defeatScreen;

    private Transform player;
    private Rigidbody playerRb;

    private bool hasSeenPlayer;
    private float lastAttackTime;

    private static int playerHits = 0;

    void Start()
    {
        currentLife = lifeMax;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
            playerRb = playerObj.GetComponent<Rigidbody>();
        }

        if (defeatScreen != null)
            defeatScreen.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float distance =
            Vector3.Distance(transform.position, player.position);

        // Detecta o player
        if (distance <= followDistance)
            hasSeenPlayer = true;

        if (!hasSeenPlayer) return;

        FollowPlayer();
    }

    private void FollowPlayer()
    {
        // DIREÇÃO
        Vector3 direction =
            (player.position - transform.position).normalized;

        // MOVIMENTO
        Vector3 newPosition =
            transform.position + direction * speed * Time.deltaTime;

        newPosition.y = transform.position.y;

        transform.position = newPosition;

        // ROTAÇÃO SUAVE
        Vector3 lookDirection =
            player.position - transform.position;

        lookDirection.y = 0f;

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(lookDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public void Hitted(float damage, Vector3 hitPoint)
    {
        currentLife -= damage;

        SpawnBlood(hitPoint);

        if (currentLife <= 0)
            Die();
    }

    private void SpawnBlood(Vector3 hitPoint)
    {
        if (bloodEffect == null) return;

        // Faz o sangue olhar para o player
        Vector3 directionToPlayer =
            (player.position - hitPoint).normalized;

        Quaternion rotation =
            Quaternion.LookRotation(directionToPlayer);

        GameObject blood = Instantiate(
            bloodEffect,
            hitPoint,
            rotation
        );

        // Tamanho do sangue
        blood.transform.localScale =
            Vector3.one * bloodSize;

        // Faz acompanhar o inimigo
        blood.transform.SetParent(transform);

        // Mantém posição correta
        blood.transform.position = hitPoint;

        ParticleSystem ps =
            blood.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            ps.Play();

            Destroy(
                blood,
                ps.main.duration +
                ps.main.startLifetime.constantMax
            );
        }
        else
        {
            Destroy(blood, 2f);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // Cooldown entre ataques
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        AttackPlayer();
    }

    private void AttackPlayer()
    {
        // Knockback
        if (playerRb != null)
        {
            Vector3 knockbackDir =
                (player.position - transform.position).normalized;

            knockbackDir.y = 0.5f;

            playerRb.AddForce(
                knockbackDir * knockbackForce,
                ForceMode.Impulse
            );
        }

        playerHits++;

        Debug.Log("Player atingido: " + playerHits);

        // Game Over
        if (playerHits >= 3)
        {
            if (defeatScreen != null)
                defeatScreen.SetActive(true);

            Time.timeScale = 0f;
        }
    }
}