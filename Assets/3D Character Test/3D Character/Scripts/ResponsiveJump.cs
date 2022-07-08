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
          switch (_rb.velocity.y)
          {
               case < 0f:
                    _rb.velocity += Vector3.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
                    break;
               case > 0f when !InputsController.jumping:
                    _rb.velocity += Vector3.up * (Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
                    break;
          }
     }
}
