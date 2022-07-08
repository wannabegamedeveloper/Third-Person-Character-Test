using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float xSensitivity;
    [SerializeField] private float ySensitivity;
    [SerializeField] private Transform point;
    [SerializeField] private Transform player;
    [SerializeField] private float minCamAngle;
    [SerializeField] private float maxCamAngle;
    
    private Transform _transform;
    private Vector2 _mouseInput;

    private void Start()
    {
        _transform = player;
    }

    private void Update()
    {
        var position = _transform.position;
        var pointPosition = transform;

        pointPosition.position = Vector3.Lerp(pointPosition.position, position, 100f * Time.deltaTime);

        float mouseX = InputsController.Mouse().y;
        float mouseY = InputsController.Mouse().x;

        point.Rotate(new Vector3(-mouseX / 20f * ySensitivity, 0f, 0f));
        transform.Rotate(new Vector3(0f, mouseY / 20f * xSensitivity, 0f));

        var rot = point.localRotation;
        rot.x = Mathf.Clamp(rot.x, minCamAngle, maxCamAngle);
        point.localRotation = rot;
    }
}
