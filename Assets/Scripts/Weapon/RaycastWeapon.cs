using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    public enum WeaponCode
    {
        Handgun = 1,
        Rifle = 5
    }

    [Header("FX")]
    [SerializeField] private ParticleSystem _muzzleFlash;

    [Header("Information")]
    [SerializeField] private Transform _firePoint;
    [HideInInspector] public Transform aimLookAt;
    [SerializeField] private string _weaponName;
    [SerializeField] private float _damageAmount;
    [SerializeField] private WeaponCode _weaponType;
    public WeaponCode WeaponType { get { return _weaponType; } }
    [SerializeField] private PlayerWeapon.WeaponSlot _weaponSlot;
    public PlayerWeapon.WeaponSlot WeaponSlot { get { return _weaponSlot; } }
    [SerializeField] private float _weaponCooldown;
    public float WeaponCooldown { get { return _weaponCooldown; } }

    [Header("Behaviors")]
    [SerializeField] private int _ammoCount;
    public int AmmoCount { get { return _ammoCount; } }
    [SerializeField] private int _clipSize;

    [SerializeField] private float reloadTime = 1f;
    public float ReloadTime { get { return reloadTime; } }

    [HideInInspector] public bool isHolstered;
    [HideInInspector] public bool IsReloading;

    // Start is called before the first frame update
    void Start()
    {
        _ammoCount = _clipSize;
    }

    public void InitAmmo()
    {
        _ammoCount = _clipSize;
    }

    public IEnumerator Reload()
    {
        if (_ammoCount == _clipSize)
            yield return null;

        IsReloading = true;

        float elapsedTime = 0;
        while (elapsedTime <= reloadTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        FillAmmo();
        IsReloading = false;
    }

    public void FillAmmo()
    {
        _ammoCount = _clipSize;
    }

    void ReduceAmmo()
    {
        if (_ammoCount <= 0)
            return;
        _ammoCount--;
    }

    public void OnFireBullets()
    {
        _muzzleFlash.Emit(1);
        var fireDirection = aimLookAt.position - _firePoint.position;
        GameObject bullet = ObjectPooler.Singleton.GetBullet();
        bullet.transform.position = _firePoint.position;
        bullet.transform.forward = fireDirection;
        bullet.GetComponent<Bullet>().damage = _damageAmount;
        bullet.SetActive(true);

        ReduceAmmo();
    }

    public bool CheckOutOfAmmo()
    {
        return _ammoCount <= 0;
    }
}
