using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector3 _movement;
    private CharacterController _controller;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        _movement = new Vector3(moveX, 0f, moveY);
        Vector3 localMovement = transform.TransformDirection(_movement);
        
        _controller.Move(localMovement * speed * Time.deltaTime);
    }
}
