using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float timer = 2f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float damage = 3f;

    [Header("Efeitos")]
    [SerializeField] private GameObject explosionEffect;

    private void Start()
    {
        Invoke(nameof(Explode), timer);
    }

    void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(
                explosionEffect,
                transform.position,
                Quaternion.identity
            );
        }

        Collider[] hits =
            Physics.OverlapSphere(transform.position, radius);

        foreach (Collider hit in hits)
        {
            IShootable shootable =
                hit.GetComponent<IShootable>();

            if (shootable != null)
            {
                shootable.Hitted(
                    damage,
                    hit.transform.position
                );
            }
        }

        CameraShake.Instance.Shake(0.2f, 0.15f);

        Destroy(gameObject);
    }
}