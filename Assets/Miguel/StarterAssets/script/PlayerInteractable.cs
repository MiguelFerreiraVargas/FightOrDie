using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    private GunSystem _gunSystem;
    void Start()
    {
        _gunSystem = GetComponentInParent<GunSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out Icollectable collectable))
            return;

        switch (other.gameObject.tag)
        {
            case "Gun":
                 _gunSystem.AddNewGun((GunElement)collectable.GetItem());
                break;
            case "Ammo":
                break;
            case "Armor":
                break;
            default:
                break;
        }

    }
}
