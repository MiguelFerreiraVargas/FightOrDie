using UnityEngine;
using StarterAssets;

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
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Efeitos")]
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private float bloodSize = 2f;

    private Transform player;

    private bool hasSeenPlayer;
    private float lastAttackTime;

    private static int playerHits = 0;

    void Start()
    {
        playerHits = 0;

        currentLife = lifeMax;

        GameObject playerObj =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
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

        // ATAQUE
        if (distance <= attackDistance)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;

                AttackPlayer();
            }
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction =
            (player.position - transform.position).normalized;

        Vector3 newPosition =
            transform.position + direction * speed * Time.deltaTime;

        newPosition.y = transform.position.y;

        transform.position = newPosition;

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

        Vector3 directionToPlayer =
            (player.position - hitPoint).normalized;

        Quaternion rotation =
            Quaternion.LookRotation(directionToPlayer);

        GameObject blood = Instantiate(
            bloodEffect,
            hitPoint,
            rotation
        );

        blood.transform.localScale =
            Vector3.one * bloodSize;

        blood.transform.SetParent(transform);

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
        GameManager.Instance.AddKill();

        Destroy(gameObject);
    }

    private void AttackPlayer()
    {
        // KNOCKBACK
        FirstPersonController controller =
            player.GetComponent<FirstPersonController>();

        if (controller != null)
        {
            Vector3 knockbackDir =
                -player.forward;

            knockbackDir.y = 0.2f;

            knockbackDir.Normalize();

            controller.KnockbackVelocity =
                knockbackDir * knockbackForce;
        }

        playerHits++;

        Debug.Log("Player atingido: " + playerHits);

        // GAME OVER
        if (playerHits >= 4)
        {
            FirstPersonController movement =
                player.GetComponent<FirstPersonController>();

            if (movement != null)
                movement.enabled = false;

            DefeatManager.Instance.ShowDefeat();
        }
    }
}