using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public RaycastWeapon weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        PlayerWeapon activeWeapon = other.gameObject.GetComponent<PlayerWeapon>();
        if (activeWeapon)
        {
            RaycastWeapon newWeapon = Instantiate(weaponPrefab);
            newWeapon.InitAmmo();
            activeWeapon.Equip(newWeapon);

            Destroy(gameObject, 0.25f);
        }
    }
}
