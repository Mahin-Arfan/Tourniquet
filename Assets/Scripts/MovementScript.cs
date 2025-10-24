using Cinemachine;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class MovementScript : MonoBehaviour
{
    [Header("References")]
    public CinemachineVirtualCamera cinemachineCam;
    public Transform headBone;
    public Transform upperSpine;        // Spine bone (upper)
    public Transform lowerSpine;        // Spine bone (lower)
    public SkinnedMeshRenderer headRenderer; // Head mesh renderer to hide locally

    [Header("Movement Settings")]
    public float moveSpeed = 4f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private float pitch; // camera rotation (X axis)
    private float yaw;   // body rotation (Y axis)

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Hide head for local player only
        /*if (IsLocalPlayer())
        {
            headRenderer.enabled = false;
        }*/
    }

    void Update()
    {
        //if (!IsLocalPlayer()) return;

        HandleMovement();
        HandleCameraRotation();
        ApplySpineRotation();
    }

    bool IsLocalPlayer()
    {
        // For now return true; replace with multiplayer check like:
        // return GetComponent<NetworkIdentity>().isLocalPlayer;
        return true;
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Animator blending
        animator.SetFloat("moveX", horizontal, 0.1f, Time.deltaTime);
        animator.SetFloat("moveY", vertical, 0.1f, Time.deltaTime);
    }

    void HandleCameraRotation()
    {
        // Use Cinemachine POV for rotation, but we still sync bones
        Transform cam = cinemachineCam.VirtualCameraGameObject.transform;
        Vector3 euler = cam.localRotation.eulerAngles;

        // Extract pitch (x rotation)
        pitch = euler.x;
        yaw = transform.eulerAngles.y;

        // Rotate body toward camera yaw
        transform.rotation = Quaternion.Euler(0, euler.y, 0);
    }

    void ApplySpineRotation()
    {
        // Slightly rotate spine and head to follow pitch
        float pitchOffset = pitch > 180 ? pitch - 360 : pitch; // convert to -180~180

        Quaternion headRot = Quaternion.Euler(pitchOffset * 1.0f, 0, 0);
        Quaternion spineRot = Quaternion.Euler(pitchOffset * 0.5f, 0, 0);
        Quaternion lowerSpineRot = Quaternion.Euler(pitchOffset * 0.25f, 0, 0);

        if (headBone)
            headBone.localRotation = headRot;

        if (upperSpine)
            upperSpine.localRotation = spineRot;

        if (lowerSpine)
            lowerSpine.localRotation = lowerSpineRot;
    }
}
