using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Animator _playerAnimator;
    [SerializeField] Transform _weaponHolder;
    [SerializeField] Transform _cameraLookAt;
    private Transform _player;
    private float _rotationOnX;
    private float _mouseSensitivity = 100f;

    int _headHorizontalHash = Animator.StringToHash("Head_Horizontal_f");
    int _headVerticalHash = Animator.StringToHash("Head_Vertical_f");
    int _bodyHorizontalHash = Animator.StringToHash("Body_Horizontal_f");
    int _bodyVerticalHash = Animator.StringToHash("Body_Vertical_f");

    // Start is called before the first frame update
    void Start()
    {
        _player = transform.parent;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance == null) return;

        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        
        // Rotate camera up and down
        _rotationOnX -= mouseY;
        _rotationOnX = Mathf.Clamp(_rotationOnX, -80f, 70f);
        transform.localEulerAngles = new Vector3(_rotationOnX, 0, 0);
        _weaponHolder.localEulerAngles = new Vector3(_rotationOnX, 0, 0);

        // Rotate player left and right
        _player.Rotate(Vector3.up * mouseX);

        // Head and Body Look at
        HeadAndBodyAnimate(_rotationOnX / -90);
    }

    void HeadAndBodyAnimate(float value)
    {
        _playerAnimator.SetFloat(_headVerticalHash, value);
        _playerAnimator.SetFloat(_bodyVerticalHash, value);
    }

    public void FreeCamera()
    {
        transform.SetParent(null);
    }
}
