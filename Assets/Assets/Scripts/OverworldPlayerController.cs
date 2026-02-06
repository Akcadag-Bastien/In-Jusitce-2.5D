using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class OverworldPlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float damping = 14f;

    private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
        body.constraints = RigidbodyConstraints.FreezePositionY;
    }

    private void FixedUpdate()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        if (input.sqrMagnitude > 1f)
        {
            input.Normalize();
        }

        Vector3 currentHorizontalVelocity = new Vector3(body.velocity.x, 0f, body.velocity.z);
        Vector3 targetVelocity = input * moveSpeed;

        Vector3 newHorizontalVelocity = Vector3.MoveTowards(
            currentHorizontalVelocity,
            targetVelocity,
            acceleration * Time.fixedDeltaTime);

        if (input.sqrMagnitude < 0.01f)
        {
            newHorizontalVelocity = Vector3.MoveTowards(
                newHorizontalVelocity,
                Vector3.zero,
                damping * Time.fixedDeltaTime);
        }

        body.velocity = new Vector3(newHorizontalVelocity.x, 0f, newHorizontalVelocity.z);
    }
}
