using System;
using Cinemachine;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float animationTransitionTime;
    [SerializeField] private Transform constraint;
    [SerializeField] private float bendAmount;
    [SerializeField] private Transform followPoint;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    [SerializeField] private bool isGrounded;
    [SerializeField] private AudioSource running;
    [SerializeField] private AudioSource fall;
    [SerializeField] private AudioSource doubleJump;
    [SerializeField] private bool doubleJumped;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera; 
    [SerializeField] private float noiseIntensity;
    [SerializeField] private float noiseFactor;
    [SerializeField] private float fov;
    
    private CinemachineBasicMultiChannelPerlin _noise;
    private Vector3 _movement;
    private Rigidbody _rb;
    private Animator _characterAnimator;
    private float _lerpingSpeedX;
    private float _lerpingSpeedY;
    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Y = Animator.StringToHash("Y");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private float _performJump;
    private static readonly int JumpEnd = Animator.StringToHash("Jump End");
    private static readonly int DoubleJump = Animator.StringToHash("Double Jump");
    private bool _inAir;
    private SmolCharacter _smolCharacter;

    private void Awake()
    {
        _smolCharacter = new SmolCharacter();
    }

    private void OnEnable()
    {
        _smolCharacter.Enable();
    }

    private void OnDisable()
    {
        _smolCharacter.Disable();
    }

    private void Start()
    {
        _noise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _rb = GetComponent<Rigidbody>();
        _characterAnimator = GetComponent<Animator>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Animate(Movement());
    }

    private void Animate(Vector2 axis)
    {
        _lerpingSpeedX = Mathf.Lerp(_lerpingSpeedX, axis.x, animationTransitionTime * Time.deltaTime);
        _lerpingSpeedY = Mathf.Lerp(_lerpingSpeedY, axis.y, animationTransitionTime * Time.deltaTime);

        _characterAnimator.SetFloat(X, _lerpingSpeedX);
        _characterAnimator.SetFloat(Y, _lerpingSpeedY);
    }

    private Vector2 Movement()
    {
        Vector2 move = _smolCharacter.Player.Movement.ReadValue<Vector2>();

        if (Mathf.Abs(move.x) > 0f || Mathf.Abs(move.y) > 0f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, followPoint.localRotation, 10f * Time.deltaTime);
            
            float mouseY = _smolCharacter.Player.MouseX.ReadValue<float>();
            
            if (mouseY > 3f) return new Vector2(move.x, move.y);
            
            constraint.localRotation =
                Quaternion.Lerp(constraint.localRotation, Quaternion.Euler(0f, 0f, mouseY * -bendAmount),
                    3f * Time.deltaTime);
        }
        else
            constraint.localRotation = Quaternion.identity;
        
        //Jumping();
        //DoubleJumping();
        
        _movement = new Vector3(move.x, 0f, move.y);
        Vector3 localMovement = transform.TransformDirection(_movement) * speed;
        Vector3 vel = _rb.velocity;
        vel.x = localMovement.x;
        vel.z = localMovement.z;
        _rb.velocity = vel;

        if (!isGrounded)
        {
            _noise.m_AmplitudeGain = noiseIntensity * noiseFactor;
            cinemachineVirtualCamera.m_Lens.FieldOfView = fov;
        }

        return new Vector2(move.x, move.y);
    }

    private void Jumping()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || !isGrounded || doubleJumped) return;
        running.volume = 0f;
        ApplyJumpForce(jumpForce);
    }

    private void DoubleJumping()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || isGrounded || doubleJumped) return;
        ApplyJumpForce(doubleJumpForce);
        doubleJump.PlayOneShot(doubleJump.clip);
        _characterAnimator.SetTrigger(DoubleJump);
    }

    private void ApplyJumpForce(float force)
    {
        _rb.velocity = Vector3.up * force;
    }

    private void OnTriggerExit(Collider other)
    {
        _characterAnimator.SetTrigger(Jump);
        isGrounded = false;
        _characterAnimator.ResetTrigger(JumpEnd);
    }

    private void OnTriggerEnter(Collider other)
    {
        running.volume = 1f;
        _characterAnimator.SetTrigger(JumpEnd);
        fall.PlayOneShot(fall.clip);
        isGrounded = true;
    }
}
