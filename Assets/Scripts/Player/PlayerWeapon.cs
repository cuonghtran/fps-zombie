using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeapon : MonoBehaviour
{
    public enum WeaponSlot
    {
        Primary = 0,
        Secondary = 1
    }

    [Header("References")]
    [SerializeField] Transform _aimLookAt;
    [SerializeField] Transform[] _weaponSlots;
    [SerializeField] private int activeWeaponIndex = -1;

    RaycastWeapon activeWeapon;
    RaycastWeapon[] equippedWeapons = new RaycastWeapon[2];
    bool isHolstered;
    bool _onCooldown;
    float _weaponCooldownTime = 0f;

    private Animator _animator;

    int _weaponTypeHash = Animator.StringToHash("WeaponType_int");

    public static Action<float> OnReload;
    public static Action<int> OnAmmoChanged;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        ChangeWeapon();
        StartCoroutine(ReloadActiveWeapon());
        WeaponFireHandler();
    }

    void ChangeWeapon()
    {
        if (activeWeapon && !activeWeapon.IsReloading)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetActiveWeapon(WeaponSlot.Primary);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetActiveWeapon(WeaponSlot.Secondary);
            }
        }
    }

    private void WeaponFireHandler()
    {
        if (activeWeapon && !_onCooldown && !activeWeapon.IsReloading)
        {
            if (Input.GetMouseButton(0))
            {
                if (Time.time >= _weaponCooldownTime)
                {
                    _weaponCooldownTime = Time.time + activeWeapon.WeaponCooldown;
                    activeWeapon.OnFireBullets();
                    UpdateAmmoUI();
                }
            }
        }
    }

    IEnumerator ReloadActiveWeapon()
    {
        if (activeWeapon && (Input.GetKeyDown(KeyCode.R) || activeWeapon.CheckOutOfAmmo()))
        {
            OnReload?.Invoke(activeWeapon.ReloadTime);
            yield return StartCoroutine(activeWeapon.Reload());
            UpdateAmmoUI();
        }
    }

    void UpdateAmmoUI()
    {
        OnAmmoChanged?.Invoke(activeWeapon.AmmoCount);
    }

    RaycastWeapon GetWeaponByIndex(int index)
    {
        if (index < 0 || index >= equippedWeapons.Length)
            return null;
        return equippedWeapons[index];
    }

    public RaycastWeapon GetActiveWeapon()
    {
        return GetWeaponByIndex(activeWeaponIndex);
    }

    public void Equip(RaycastWeapon newWeapon)
    {
        int weaponSlotIndex = (int)newWeapon.WeaponSlot;
        var weapon = GetWeaponByIndex(weaponSlotIndex);
        if (weapon)
            return; // do nothing if pickup an already owned weapon

        weapon = newWeapon;
        weapon.aimLookAt = _aimLookAt;
        weapon.transform.SetParent(_weaponSlots[weaponSlotIndex], false);
        equippedWeapons[weaponSlotIndex] = weapon;

        SetActiveWeapon(newWeapon.WeaponSlot);
        _animator.SetInteger(_weaponTypeHash, (int)weapon.WeaponType);
    }

    void SetActiveWeapon(WeaponSlot weaponSlot)
    {
        int newWeaponIndex = (int)weaponSlot;
        if (activeWeaponIndex == newWeaponIndex || equippedWeapons[newWeaponIndex] == null)
            return;

        activeWeaponIndex = newWeaponIndex;
        _weaponCooldownTime = 0;
        StartCoroutine(SwitchWeapon(newWeaponIndex));
    }

    IEnumerator HolsterCurrentWeapon()
    {
        _onCooldown = true;

        foreach (RaycastWeapon wp in equippedWeapons)
        {
            if (wp != null) wp.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.05f);
    }

    IEnumerator ActivateWeapon(int index, bool fromPickUp)
    {
        var weapon = GetWeaponByIndex(index);
        if (weapon)
        {
            activeWeapon = weapon;
            weapon.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.05f);
            UpdateAmmoUI();
            _onCooldown = false;
        }
    }

    IEnumerator SwitchWeapon(int newWeaponIndex, bool fromPickUp = false)
    {
        yield return StartCoroutine(HolsterCurrentWeapon());
        yield return StartCoroutine(ActivateWeapon(newWeaponIndex, fromPickUp));
    }
}
