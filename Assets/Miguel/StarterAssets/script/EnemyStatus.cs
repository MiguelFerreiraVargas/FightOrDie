using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyStatus : MonoBehaviour, IShootable
{
    [Header("Vida")]
    [SerializeField] private float _lifeMax = 2;
    private float _currentLife;

    [Header("Efeito")]
    [SerializeField] private GameObject _bloodEffect;

    [Header("Perseguição")]
    [SerializeField] private float _followDistance = 10f;
    [SerializeField] private float _speed = 3f;

    [Header("Ataque")]
    [SerializeField] private float _knockbackForce = 7f;
    [SerializeField] private float _attackCooldown = 1f;

    [Header("Derrota")]
    [SerializeField] private GameObject _defeatScreen;

    private Transform _player;
    private Rigidbody _playerRb;

    private float _lastAttackTime;

    private static int _playerHits = 0;

    // Depois que vê o player uma vez,
    // segue para sempre
    private bool _hasSeenPlayer = false;

    public void Hitted(float damage, Vector3 hitPoint)
    {
        _currentLife -= damage;

        GameObject blood = Instantiate(
            _bloodEffect,
            hitPoint,
            Quaternion.identity
        );

        // garante que aparece antes de qualquer coisa
        blood.transform.position = hitPoint;

        // NÃO deixa herdar rotação nem escala bugada
        blood.transform.SetParent(null);

        // força o Particle System a tocar
        ParticleSystem ps = blood.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            Destroy(blood, ps.main.duration);
        }
        else
        {
            Destroy(blood, 2f);
        }

        if (_currentLife <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _currentLife = _lifeMax;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            _player = playerObj.transform;
            _playerRb = playerObj.GetComponent<Rigidbody>();
        }

        if (_defeatScreen != null)
            _defeatScreen.SetActive(false);
    }

    void Update()
    {
        if (_player == null)
            return;

        float distance =
            Vector3.Distance(transform.position, _player.position);

        // Detecta o player uma vez
        if (distance <= _followDistance)
        {
            _hasSeenPlayer = true;
        }

        // Segue para sempre
        if (_hasSeenPlayer)
        {
            Vector3 direction =
                (_player.position - transform.position).normalized;

            Vector3 newPosition =
                transform.position + direction * _speed * Time.deltaTime;

            // Impede o inimigo de subir/descer
            newPosition.y = transform.position.y;

            transform.position = newPosition;

            // Faz o inimigo olhar para o player
            Vector3 targetPosition = new Vector3(
                _player.position.x,
                transform.position.y,
                _player.position.z
            );

            transform.LookAt(targetPosition);

            // Corrige inimigo andando de costas
            transform.Rotate(0, 180f, 0);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // Cooldown entre ataques
        if (Time.time < _lastAttackTime + _attackCooldown)
            return;

        _lastAttackTime = Time.time;

        // Knockback no player
        if (_playerRb != null)
        {
            Vector3 knockbackDir =
                (_player.position - transform.position).normalized;

            knockbackDir.y = 0.5f;

            _playerRb.AddForce(
                knockbackDir * _knockbackForce,
                ForceMode.Impulse
            );
        }

        // Conta hits
        _playerHits++;

        Debug.Log("Player atingido: " + _playerHits);

        // Game Over após 3 hits
        if (_playerHits >= 3)
        {
            if (_defeatScreen != null)
            {
                _defeatScreen.SetActive(true);
            }

            Time.timeScale = 0f;
        }
    }
}