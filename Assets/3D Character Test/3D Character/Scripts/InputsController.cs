using UnityEngine;

public class InputsController : MonoBehaviour
{
    private static SmolCharacter _smolCharacter;

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
}
