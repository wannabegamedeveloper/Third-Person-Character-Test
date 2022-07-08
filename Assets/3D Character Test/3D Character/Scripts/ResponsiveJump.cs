using UnityEngine;

public class ResponsiveJump : MonoBehaviour
{
     [SerializeField] private float fallMultiplier;
     [SerializeField] private float lowJumpMultiplier;

     private Rigidbody _rb;

     private void Awake()
     {
          _rb = GetComponent<Rigidbody>();
     }

     private void Update()
     {
          if (_rb.velocity.y < 0f)
               _rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
          else if (_rb.velocity.y > 0f && !Input.GetKeyDown(KeyCode.Space))
               _rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
     }
}
