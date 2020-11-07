using UnityEngine;

public class FPSInput : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the main camera used for the player")]
    [SerializeField] private Camera playerCamera = null;
    [Tooltip("Reference to the footstep sound used for the player")]
    [SerializeField] private AudioSource footstepSound = null;

    private float cameraVerticalAngle = 0f;
    const float lookSensitivity = 220.0f;
    // This will be changed if the player is aiming
    const float RotationMultiplier = 1.0f;
    private bool popupsOpen = false;

    private float speed = 9.0f;

    private float yVelocity = 0f;
    private float gravity = -9.8f;
    private int maxJumps = 1;
    private int currJumps = 0;
    private float jumpForce = 5.0f;
    private float pushForce = 5.0f;

    float footstepDistanceCounter = 0f;
    float footstepSFXFrequency = 0.3f;

    private CharacterController charController;

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    private void Awake()
    {
        Messenger<int>.AddListener(GameEvent.UI_POPUP_OPENED, popupsChanged);
        Messenger<int>.AddListener(GameEvent.UI_POPUP_CLOSED, popupsChanged);
    }
    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.UI_POPUP_OPENED, popupsChanged);
        Messenger<int>.RemoveListener(GameEvent.UI_POPUP_CLOSED, popupsChanged);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        // Does it have a rigidBody and is physics enabled
        if (body != null && !body.isKinematic)
        {
            body.velocity = hit.moveDirection * pushForce;
        }
    }

    private float getMouseInput(string axis)
    {
        if(popupsOpen)
        {
            return 0;
        }

        float input = Input.GetAxisRaw(axis);

        // Reduce mouse input amount to be equivalent to stick movement
        // Note: this is for controller support from FPS microgame
        input *= 0.01f;

        #if UNITY_WEBGL
            // Mouse tends to be even more sensitive in WebGL due to mouse acceleration, so reduce it even more
            input *= webglLookSensitivityMultiplier;
        #endif

        return input;
    }

    private void popupsChanged(int numPopups)
    {
        popupsOpen = numPopups > 0;
    }

    // Update is called once per frame
    void Update()
    {
        // horizontal character rotation
        {
            // Rotate the transform with the input speed around its local Y axis
            transform.Rotate(new Vector3(0f, (getMouseInput("Mouse X") * lookSensitivity * RotationMultiplier), 0f), Space.Self);
        }

        // vertical camera rotation
        {
            // Add vertical inputs to the camera's vertical angle
            cameraVerticalAngle -= getMouseInput("Mouse Y") * lookSensitivity * RotationMultiplier;

            // Limit the camera's vertical angle to min/max
            cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);

            // Apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
            playerCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);
        }

        // Character Movement
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);

        // Clamp magnitude for diagonal movement
        movement = Vector3.ClampMagnitude(movement, speed);
        Vector3 baseMovement = movement;

        // Add jump and gravity
        if (charController.isGrounded)
        {
            yVelocity = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                currJumps++;
                yVelocity = jumpForce;
            } else
            {
                currJumps = 0;
            }
        } else if (currJumps < maxJumps && Input.GetButtonDown("Jump")) {
            currJumps++;
            yVelocity = jumpForce;
        }
        yVelocity += gravity * Time.deltaTime;
        movement.y = yVelocity;

        // Movement code Frame Rate Independent
        movement *= Time.deltaTime;

        // Convert local to global coordinates
        movement = transform.TransformDirection(movement);

        charController.Move(movement);

        // Play footstepSound if moving and grounded
        if (charController.isGrounded && (Mathf.Abs(deltaX) > 0.2 || Mathf.Abs(deltaZ) > 0.2))
        {
            
            if (footstepDistanceCounter >= 1f / footstepSFXFrequency)
            {
                footstepDistanceCounter = 0f;
                footstepSound.Play();
            }

            // keep track of distance traveled for footsteps sound
            footstepDistanceCounter += baseMovement.magnitude * Time.deltaTime;
        }
    }
}
