using UnityEngine;

public class KnifeMelee : MonoBehaviour
{
    [SerializeField] private float damage = 2f;
    [SerializeField] private float range = 2f;
    [SerializeField] private Camera cam;

    [Header("Efeitos")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource hitSound;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Attack()
    {
        animator?.SetTrigger("Attack");

        CameraShake.Instance.Shake(0.05f, 0.03f);

        Ray ray =
            cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            IShootable shootable =
                hit.collider.GetComponent<IShootable>();

            if (shootable != null)
            {
                shootable.Hitted(damage, hit.point);

                hitSound?.Play();
            }
        }
    }
}