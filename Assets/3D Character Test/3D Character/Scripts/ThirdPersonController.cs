using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float animationTransitionTime;
    [SerializeField] private Transform constraint;
    [SerializeField] private float bendAmount;
    [SerializeField] private Transform followPoint;
    
    private Vector3 _movement;
    private CharacterController _controller;
    private Animator _characterAnimator;
    private float _lerpingSpeed;
    private static readonly int X = Animator.StringToHash("X");
    
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _characterAnimator = GetComponent<Animator>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Movement();
        Animate();
    }

    private void Animate()
    {
        Vector3 characterVelocity = _controller.velocity;
        float normalizedVelocity = Vector3.Magnitude(Vector3.Normalize(characterVelocity));

        _lerpingSpeed = Mathf.Lerp(_lerpingSpeed, normalizedVelocity, animationTransitionTime * Time.deltaTime);
        
        _characterAnimator.SetFloat(X, _lerpingSpeed);
    }

    private void Movement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if (Mathf.Abs(moveX) > 0f || Mathf.Abs(moveY) > 0f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, followPoint.localRotation, 10f * Time.deltaTime);
            
            float mouseY = Input.GetAxis("Mouse X");
            
            if (mouseY > 3f) return;
            
            constraint.localRotation =
                Quaternion.Lerp(constraint.localRotation, Quaternion.Euler(0f, 0f, mouseY * -bendAmount),
                    10f * Time.deltaTime);
        }
        else
            constraint.localRotation = Quaternion.identity;
        
        _movement = new Vector3(moveX, 0f, moveY);
        Vector3 localMovement = transform.TransformDirection(_movement);
        
        _controller.Move(localMovement * (speed * Time.deltaTime));
    }
    
}
