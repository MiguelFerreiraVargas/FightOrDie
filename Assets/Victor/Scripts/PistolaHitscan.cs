using UnityEngine;

public class PistolaHitscan : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float damage = 1f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 0.2f;

    [Header("Referências")]
    [SerializeField] private Camera cam;
    [SerializeField] private Transform shootPoint;

    [Header("Efeitos")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject bulletImpact;
    [SerializeField] private LineRenderer bulletTrail;
    [SerializeField] private AudioSource shootSound;

    private float nextFire;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (Time.time < nextFire)
            return;

        nextFire = Time.time + fireRate;

        muzzleFlash?.Play();
        shootSound?.Play();

        CameraShake.Instance.Shake(0.08f, 0.05f);

        Ray ray =
            cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            IShootable shootable =
                hit.collider.GetComponent<IShootable>();

            if (shootable != null)
            {
                shootable.Hitted(damage, hit.point);
            }

            if (bulletImpact != null)
            {
                Instantiate(
                    bulletImpact,
                    hit.point,
                    Quaternion.LookRotation(hit.normal)
                );
            }

            DrawTrail(shootPoint.position, hit.point);
        }
        else
        {
            DrawTrail(
                shootPoint.position,
                ray.origin + ray.direction * range
            );
        }
    }

    void DrawTrail(Vector3 start, Vector3 end)
    {
        if (bulletTrail == null) return;

        LineRenderer trail =
            Instantiate(bulletTrail);

        trail.SetPosition(0, start);
        trail.SetPosition(1, end);

        Destroy(trail.gameObject, 0.05f);
    }
}