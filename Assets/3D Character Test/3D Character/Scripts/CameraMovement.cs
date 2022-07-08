using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform point;
    [SerializeField] private Transform player;
    [SerializeField] private float minCamAngle;
    [SerializeField] private float maxCamAngle;
    
    private Transform _transform;

    private SmolCharacter _smolCharacter;
    
    private void Start()
    {
        _smolCharacter = new SmolCharacter();
        _transform = player;
    }

    private void Update()
    {
        var position = _transform.position;
        var pointPosition = transform;

        pointPosition.position = Vector3.Lerp(pointPosition.position, position, 100f * Time.deltaTime);

        float mouseX = _smolCharacter.Player.CamRotation.ReadValue<Vector2>().y;
        float mouseY = _smolCharacter.Player.CamRotation.ReadValue<Vector2>().x;

        point.Rotate(new Vector3(-mouseX, 0f, 0f));
        transform.Rotate(new Vector3(0f, mouseY, 0f));

        var rot = point.localRotation;
        rot.x = Mathf.Clamp(rot.x, minCamAngle, maxCamAngle);
        point.localRotation = rot;
    }
}
