using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    ZombieBehaviors _zombieBase;

    // Start is called before the first frame update
    void Start()
    {
        _zombieBase = transform.parent.GetComponent<ZombieBehaviors>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent<Damageable>(out Damageable damageable))
        {
            var damageMsg = new Damageable.DamageMessage
            {
                amount = _zombieBase.Damage,
                damager = _zombieBase.gameObject,
                direction = other.transform.position - _zombieBase.transform.position,
                knockBackForce = _zombieBase.KnockForce
            };
            damageable.ApplyDamage(damageMsg);
        }
    }
}
