using Cinemachine;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private bool stopMovement;
    [SerializeField] private float speed;
    [SerializeField] private float animationTransitionTime;
    [SerializeField] private Transform constraint;
    [SerializeField] private float bendAmount;
    [SerializeField] private Transform followPoint;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    [SerializeField] private float bashForce;
    [SerializeField] private bool isGrounded;
    [SerializeField] private AudioSource running;
    [SerializeField] private AudioSource fall;
    [SerializeField] private AudioSource doubleJump;
    [SerializeField] private AudioSource fallBashSFX;
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
    private static readonly int Bash = Animator.StringToHash("Fall Bash");

    private void Start()
    {
        _noise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _rb = GetComponent<Rigidbody>();
        _characterAnimator = GetComponent<Animator>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        InputsController.JumpAction.AddListener(Jumping);
        InputsController.JumpAction.AddListener(DoubleJumping);
        InputsController.BashAction.AddListener(FallBash);
    }

    private void Update()
    {
        if (!stopMovement)
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
        Vector2 move = InputsController.Movement();

        if (Mathf.Abs(move.x) > 0f || Mathf.Abs(move.y) > 0f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, followPoint.localRotation, 10f * Time.deltaTime);

            float mouseY = InputsController.Mouse().x;

            mouseY = Mathf.Clamp(mouseY, -3f, 3f);

            constraint.localRotation =
                Quaternion.Lerp(constraint.localRotation, Quaternion.Euler(0f, 0f, mouseY * -bendAmount),
                    3f * Time.deltaTime);
        }
        else
            constraint.localRotation = Quaternion.identity;

        _movement = new Vector3(move.x, 0f, move.y);
        Vector3 localMovement = transform.TransformDirection(_movement) * speed;
        Vector3 vel = _rb.velocity;
        vel.x = localMovement.x;
        vel.z = localMovement.z;
        _rb.velocity = vel;

        _noise.m_AmplitudeGain = noiseIntensity * noiseFactor;
        cinemachineVirtualCamera.m_Lens.FieldOfView = fov;

        return new Vector2(move.x, move.y);
    }

    private void Jumping()
    {
        if (!isGrounded || doubleJumped) return;
        running.volume = 0f;
        ApplyVerticalForce(jumpForce);
    }

    private void DoubleJumping()
    {
        if (isGrounded || doubleJumped) return;
        ApplyVerticalForce(doubleJumpForce);
        doubleJump.PlayOneShot(doubleJump.clip);
        _characterAnimator.SetTrigger(DoubleJump);
    }

    private void FallBash()
    {
        if (isGrounded || !doubleJumped) return;
        _characterAnimator.SetTrigger(Bash);
        ApplyVerticalForce(-bashForce);
        fallBashSFX.PlayOneShot(fallBashSFX.clip);
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
        _characterAnimator.ResetTrigger(Bash);
        fall.PlayOneShot(fall.clip);
        isGrounded = true;
    }

    private void ApplyVerticalForce(float force)
    {
        _rb.velocity = Vector3.up * force;
    }
}
