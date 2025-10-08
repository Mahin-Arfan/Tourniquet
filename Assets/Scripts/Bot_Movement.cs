using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class Bot_Movement : MonoBehaviour
{
    public float movementSpeed = 4f;
    public float gravity = -9.81f;

    private CharacterController characterController;
    private Animator animator;
    private Vector3 velocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Movement Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        characterController.Move(move * movementSpeed * Time.deltaTime);

        //Gravity
        if (characterController.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        //Animator
        bool isWalking = move.magnitude > 0.1f;

        animator.SetBool("Walk", isWalking);
    }
}
