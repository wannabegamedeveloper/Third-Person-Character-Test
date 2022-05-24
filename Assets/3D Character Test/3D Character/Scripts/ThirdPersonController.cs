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
    private float _lerpingSpeedX;
    private float _lerpingSpeedY;
    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Y = Animator.StringToHash("Y");

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
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
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(moveX) > 0f || Mathf.Abs(moveY) > 0f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, followPoint.localRotation, 10f * Time.deltaTime);
            
            float mouseY = Input.GetAxis("Mouse X");
            
            if (mouseY > 3f) return new Vector2(moveX, moveY);
            
            constraint.localRotation =
                Quaternion.Lerp(constraint.localRotation, Quaternion.Euler(0f, 0f, mouseY * -bendAmount),
                    3f * Time.deltaTime);
        }
        else
            constraint.localRotation = Quaternion.identity;
        
        _movement = new Vector3(moveX, 0f, moveY);
        Vector3 localMovement = transform.TransformDirection(_movement);
        
        _controller.Move(localMovement * (speed * Time.deltaTime));
        
        return new Vector2(moveX, moveY);
    }
    
}
