using UnityEngine;

public class ResponsiveJump : MonoBehaviour
{
     [SerializeField] private float fallMultiplier;
     [SerializeField] private float lowJumpMultiplier;

     private Rigidbody rb;

     private void Awake()
     {
          rb = GetComponent<Rigidbody>();
     }

     private void Update()
     {
          if (rb.velocity.y < 0f)
               rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
          else if (rb.velocity.y > 0f && !Input.GetKeyDown(KeyCode.Space))
               rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
     }
}
