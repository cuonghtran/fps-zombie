using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public struct DamageMessage
    {
        public GameObject damager;
        public float amount;
        public Vector3 direction;
        public float knockBackForce;
    }

    public enum CharacterType
    {
        Player, Zombie
    }

    public CharacterType characterType;
    public float maxHitPoints;
    private float _currentHitPoints;
    public float CurrentHitPoints { get { return _currentHitPoints; } }
    public float invulnerabilityTime = 0f;
    public bool isInvulnerable { get; set; }
    private float _timeSinceLastHit = 0.0f;
    public GameObject damagePopupPrefab;

    public UnityEvent OnDeath, OnReceiveDamage;

    // Start is called before the first frame update
    void Start()
    {
        ResetDamage();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvulnerable)
        {
            _timeSinceLastHit += Time.deltaTime;
            if (_timeSinceLastHit > invulnerabilityTime)
            {
                _timeSinceLastHit = 0.0f;
                isInvulnerable = false;
            }
        }

        if (Input.GetKey(KeyCode.V))
        {
            var damageMsg = new Damageable.DamageMessage
            {
                amount = 12,
                damager = gameObject,
            };
            ApplyDamage(damageMsg);
        }
    }

    public void ResetDamage()
    {
        _currentHitPoints = maxHitPoints;
        isInvulnerable = false;
        _timeSinceLastHit = 0f;
    }

    public void ApplyDamage(DamageMessage data)
    {
        // already dead or invulnerable
        if (_currentHitPoints <= 0 || isInvulnerable)
            return;

        if (characterType == CharacterType.Player)
            isInvulnerable = true;
        _currentHitPoints -= data.amount;
        //ShowDamagePopupText(data.amount);
        
        if (_currentHitPoints <= 0)
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            OnReceiveDamage?.Invoke();
            if (TryGetComponent<PlayerController>(out PlayerController playerController))
            {
                playerController.KnockWhenDamaged(data.direction, data.knockBackForce);
            }
        }
    }

    public void ShowDamagePopupText(float dmg)
    {
        var topMostPos = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        Vector3 dmgPos = new Vector3(transform.position.x, transform.position.y + topMostPos, transform.position.z);

        var dmgText = Instantiate(damagePopupPrefab, dmgPos, Quaternion.identity);
        dmgText.GetComponent<DamagePopup>().SetUp(dmg, true);
    }
}
