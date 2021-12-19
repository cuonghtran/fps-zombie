using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombieBehaviors : MonoBehaviour
{
    [SerializeField] float _scanDistance = 50;
    [SerializeField] float _scanCooldown = 1;
    [SerializeField] float _attackRange = 1f;
    [SerializeField] float _damage = 10;
    public float Damage { get { return _damage; } }
    [SerializeField] float _knockForce = 7;
    public float KnockForce { get { return _knockForce; } }
    [SerializeField] int _scoreGranted = 100;

    float _scanTimer = 0;
    Vector3 _targetPosition;
    Vector3 _velocity;
    float _gravityForce = -16f;

    NavMeshAgent _agent;
    Animator _animator;

    int _isMovingHash = Animator.StringToHash("isMoving");

    public static Action<int> OnGrantScore;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        StartCoroutine(DestroyIdleZombie());
    }

    IEnumerator DestroyIdleZombie()
    {
        yield return new WaitForSeconds(120);
        ZombieSpawner.zombiesList.Remove(this.gameObject);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= _scanTimer)
        {
            _scanTimer = Time.time + _scanCooldown;
            _targetPosition = FindTarget();
        }

        // find and chase target
        if (_targetPosition != Vector3.zero)
        {
            _agent.SetDestination(_targetPosition);
            _animator.SetBool(_isMovingHash, true);
        }
        else
        {
            _agent.SetDestination(transform.position);
            _animator.SetBool(_isMovingHash, false);
        }

        // attack
        if (_targetPosition != Vector3.zero)
        {
            if (GetDistance(transform.position, _targetPosition) <= _attackRange)
            {
                _agent.SetDestination(transform.position);
            }
        }
    }

    Vector3 FindTarget()
    {
        if (PlayerController.Instance == null) return Vector3.zero;

        if (GetDistance(transform.position, PlayerController.Instance.transform.position) <= _scanDistance)
            return PlayerController.Instance.transform.position;
        else return Vector3.zero;
    }

    float GetDistance(Vector3 from, Vector3 to)
    {
        return (to - from).magnitude;
    }

    public void GrantScoreWhenDie()
    {
        OnGrantScore?.Invoke(_scoreGranted);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _scanDistance);
    }
}
