using UnityEngine;

public class GrenadeThrowable : MonoBehaviour
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float throwForce = 15f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ThrowGrenade();
        }
    }

    void ThrowGrenade()
    {
        GameObject grenade =
            Instantiate(
                grenadePrefab,
                throwPoint.position,
                throwPoint.rotation
            );

        Rigidbody rb =
            grenade.GetComponent<Rigidbody>();

        rb.AddForce(
            throwPoint.forward * throwForce,
            ForceMode.Impulse
        );
    }
}