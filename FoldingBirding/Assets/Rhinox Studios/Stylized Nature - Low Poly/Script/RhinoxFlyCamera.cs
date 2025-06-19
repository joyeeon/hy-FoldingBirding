namespace RhinoxFlyCamera
{
    using UnityEngine;

    /// <summary>
    /// A RhinoxFlyCamera  with basic WASD/QE navigation and mouse look for use in Unity scenes.
    /// </summary>
    public class RhinoxFlyCamera : MonoBehaviour
    {
        [Tooltip("Movement speed in units per second")]
        public float movementSpeed = 10f;
        [Tooltip("Multiplier when holding Shift for faster movement")]
        public float fastSpeedMultiplier = 2f;
        [Tooltip("Mouse look sensitivity")]
        public float lookSensitivity = 3f;
        [Tooltip("Invert Y axis for mouse look")]
        public bool invertY = false;

        private float yaw;
        private float pitch;

        void Start()
        {
            // Initialize rotation
            Vector3 angles = transform.eulerAngles;
            yaw = angles.y;
            pitch = angles.x;
            // Lock the cursor at start
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            // Mouse look
            yaw += Input.GetAxis("Mouse X") * lookSensitivity;
            pitch += Input.GetAxis("Mouse Y") * lookSensitivity * (invertY ? 1f : -1f);
            pitch = Mathf.Clamp(pitch, -89f, 89f);
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

            // Calculate movement speed
            float speed = movementSpeed * (Input.GetKey(KeyCode.LeftShift) ? fastSpeedMultiplier : 1f);
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            
            // Vertical movement: E to go up, Q to go down
            if (Input.GetKey(KeyCode.E)) move.y += 1f;
            if (Input.GetKey(KeyCode.Q)) move.y -= 1f;

            // Apply movement
            transform.Translate(move * speed * Time.deltaTime, Space.Self);

            // Cursor lock/unlock controls
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
            }
            if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}


