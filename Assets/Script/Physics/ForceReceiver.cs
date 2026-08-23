using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ForceReceiver : MonoBehaviour
{
    private CharacterController controller;
    public float VerticalVelocity { get; private set; }
    public Vector3 Movement => Vector3.zero + Vector3.up * VerticalVelocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    private void Update()
    {
        if (controller.isGrounded)
        {
            VerticalVelocity = 0f;
        }
        else
        {
            VerticalVelocity += Physics.gravity.y * 2 * Time.deltaTime;
        }
    }
}
