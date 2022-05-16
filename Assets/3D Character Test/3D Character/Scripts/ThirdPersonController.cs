using System;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0f, moveY);

        rb.velocity = movement * speed * Time.deltaTime * 10f;
    }
}
