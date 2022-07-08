using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputsController : MonoBehaviour
{
    public static UnityEvent jumpAction = new UnityEvent();

    private static SmolCharacter _smolCharacter;

    private void Awake()
    {
        _smolCharacter = new SmolCharacter();
    }

    private void Start()
    {
        _smolCharacter.Player.Jump.performed += Jump;
    }

    private void OnEnable()
    {
        _smolCharacter.Enable();
    }

    private void OnDisable()
    {
        _smolCharacter.Disable();
    }

    public static Vector2 Movement()
    {
        return _smolCharacter.Player.Movement.ReadValue<Vector2>();
    }

    public static Vector2 Mouse()
    {
        Vector2 mouse = new Vector2(_smolCharacter.Player.MouseX.ReadValue<float>(),
            _smolCharacter.Player.MouseY.ReadValue<float>());
        
        return mouse;
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        jumpAction.Invoke();
    }
}
