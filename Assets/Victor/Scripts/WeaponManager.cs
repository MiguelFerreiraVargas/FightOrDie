using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;

    private int currentWeapon;

    void Start()
    {
        SelectWeapon(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectWeapon(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectWeapon(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectWeapon(2);
    }

    void SelectWeapon(int index)
    {
        currentWeapon = index;

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }
    }
}