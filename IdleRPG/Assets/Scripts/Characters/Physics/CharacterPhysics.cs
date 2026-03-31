using UnityEngine;

public class CharacterPhysics : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField] private float gravityMultiplier = 2.5f;
    public float verticalVelocity = 0f;
    // knockback //
    private Vector3 knockbackVelocity = Vector3.zero;
    [SerializeField]private float knockbackDecayRate = 6f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null)
        {
            Debug.LogError("CharacterPhysics is missing a character controller component!");
        }
    }

    public void ApplyMovement(Vector3 horizontalMovement)
    {

        verticalVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;

        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -1f;
        }

        Vector3 fullMovement = horizontalMovement * Time.deltaTime;
        fullMovement.y = verticalVelocity * Time.deltaTime;

        if (knockbackVelocity.magnitude > 0.01f)
        {

            fullMovement += knockbackVelocity * Time.deltaTime;
            knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, knockbackDecayRate * Time.deltaTime);
        }

        controller.Move(fullMovement);

    }

    public void AddKnockBack(Vector3 direction, float force)
    {
        knockbackVelocity += direction.normalized * force;
    }

    public void ResetPhysics()
    {
        verticalVelocity = 0f;
        knockbackVelocity = Vector3.zero;
    }
}
