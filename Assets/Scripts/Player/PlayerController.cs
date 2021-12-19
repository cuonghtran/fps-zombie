using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private float _speed = 350;
    [SerializeField] private float _jumpHeight = 3;
    [SerializeField] private float _gravityForce = -9.81f;
    [SerializeField] private float _groundDistance = 0.3f;
    [SerializeField] private LayerMask _groundMask;
    private float _sprintSpeedModifier = 1.5f;

    private Vector2 _input;
    private Vector3 _movementDirection;
    private Vector3 _velocity;
    private float _movementSpeed;
    private bool _isPrinting;
    private bool _isGrounded;
    private bool _isKnocking;
    private float _knockTime = 0.15f;

    private Animator _animator;
    private CharacterController _characterController;

    int _staticHash = Animator.StringToHash("Static_b");
    int _speedHash = Animator.StringToHash("Speed_f");
    int _jumpHash = Animator.StringToHash("Jump_b");

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();

        _animator.SetBool(_staticHash, true);
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        MovementHandler();
        Animate();

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            Jump();

        // gravity handler
        _velocity.y += _gravityForce * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    void MovementHandler()
    {
        _input.x = Input.GetAxisRaw("Horizontal");
        _input.y = Input.GetAxisRaw("Vertical");

        _movementDirection = (transform.right * _input.x + transform.forward * _input.y).normalized;

        float speedMod = _isPrinting ? _speed * _sprintSpeedModifier : _speed;
        _characterController.Move(_movementDirection * speedMod * Time.deltaTime);

        SprintHandler();
    }

    void Animate()
    {
        _movementSpeed = Mathf.Clamp(_movementDirection.sqrMagnitude, 0f, 1f);
        _animator.SetFloat(_speedHash, _movementSpeed);
    }

    void SprintHandler()
    {
        _isPrinting = Input.GetKey(KeyCode.LeftShift);
    }

    void CheckGrounded()
    {
        _isGrounded = Physics.CheckSphere(transform.position, _groundDistance, _groundMask);

        if (_isGrounded)
            _animator.SetBool(_jumpHash, false);
        else _animator.SetBool(_jumpHash, true);

        if (_isGrounded && _velocity.y < 0)
            _velocity.y -= 2;
    }

    void Jump()
    {
        _animator.SetBool(_jumpHash, true);
        _velocity.y = Mathf.Sqrt(-2 * _gravityForce * _jumpHeight);
    }

    public void KnockWhenDamaged(Vector3 direction, float force)
    {
        _isKnocking = true;
        StartCoroutine(KnockbackSequence(direction, force));
    }

    IEnumerator KnockbackSequence(Vector3 direction, float force)
    {
        float elapsed = 0;
        while (elapsed <= _knockTime)
        {
            elapsed += Time.deltaTime;
            _characterController.Move(direction * force * Time.deltaTime);
            yield return null;
        }

        _isKnocking = false;
    }
}
