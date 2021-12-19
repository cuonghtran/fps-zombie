using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] ParticleSystem _hitEffect;
    public float damage;
    public GameObject owner;

    private float _speed = 60f;
    Rigidbody _rigidbody;

    // Start is called before the first frame update
    void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.velocity = transform.forward * _speed;

        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _hitEffect.transform.position = collision.contacts[0].point;
        _hitEffect.transform.forward = collision.contacts[0].normal;
        _hitEffect.Emit(1);
        
        if (collision.transform.TryGetComponent<Damageable>(out Damageable damageable))
        {
            var damageMsg = new Damageable.DamageMessage
            {
                amount = damage,
                damager = gameObject,
                direction = collision.transform.position - transform.position,
                knockBackForce = 2
            };
            damageable.ApplyDamage(damageMsg);
        }

        this.gameObject.SetActive(false);
    }
}
