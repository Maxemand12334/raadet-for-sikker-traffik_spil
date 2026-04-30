using UnityEngine;
using UnityEngine.InputSystem;

public class ArmController : MonoBehaviour
{
    public Camera cam;

    [Header("Rotation Settings")]
    private float targetRotation;
    public float targetRotationSpeed = 100f;
    public float rotationSpeed = 5f;

    [Header("Movement Settings")]
    private Vector3 localTargetPosition; 
    public float targetPositionSpeed = 5f;
    public float positionSpeed = 5f;

    [Header("Floating Animation Settings")]
    public Vector3 idleLocalPos; // Where the arm sits normally
    public float offScreenHorizontalOffset = 2f; // How far to the side it hides

    public Key rotateLeftKey = Key.A;
    public Key rotateRightKey = Key.D;
    public Key moveAwayKey = Key.W;
    public Key moveCloserKey = Key.S;

    void Start()
    {
        targetRotation = this.transform.localEulerAngles.x;
        
        if (cam != null)
        {
            // 1. Set the arm to be far to the RIGHT initially
            transform.position = cam.transform.TransformPoint(idleLocalPos + Vector3.right * offScreenHorizontalOffset);
            
            // 2. Set the target to the IDLE position so it starts sliding in immediately
            localTargetPosition = idleLocalPos;
        }
    }

    // Public method to make the arm float away to the right
    public void FloatOutRight()
    {
        // Change the target to be off-screen to the right
        localTargetPosition = idleLocalPos + Vector3.right * offScreenHorizontalOffset;
    }

    // Public method to bring it back (Left to Right transition)
    public void FloatInLeft()
    {
        // Snap the physical position to the far LEFT first
        transform.position = cam.transform.TransformPoint(idleLocalPos + Vector3.left * offScreenHorizontalOffset);
        
        // Set target to idle so it slides in from the left
        localTargetPosition = idleLocalPos;
    }

    void Update()
    {
        if (cam == null) return;

        HandleInput();
        ApplyRotation();
        ApplyMovement();
    }

    void HandleInput()
    {
        // --- Rotation Input ---
        if (Keyboard.current[rotateLeftKey].isPressed && !Keyboard.current[rotateRightKey].isPressed)
        {
            targetRotation += targetRotationSpeed * Time.deltaTime;
        }
        else if (Keyboard.current[rotateRightKey].isPressed)
        {
            targetRotation -= targetRotationSpeed * Time.deltaTime;
        }

        // --- Movement Input (Modifying the local offset) ---
        // Since we move along the Camera's forward, we just change the Z of the local offset
        if (Keyboard.current[moveAwayKey].isPressed && !Keyboard.current[moveCloserKey].isPressed)
        {
            localTargetPosition += Vector3.forward * targetPositionSpeed * Time.deltaTime;
        }
        else if (Keyboard.current[moveCloserKey].isPressed)
        {
            localTargetPosition -= Vector3.forward * targetPositionSpeed * Time.deltaTime;
        }
    }

    void ApplyRotation()
    {
        float currentX = transform.localEulerAngles.x;
        // Use MoveTowards for the final snap to prevent infinite Lerp micro-movements
        float newX = Mathf.LerpAngle(currentX, targetRotation, Time.deltaTime * rotationSpeed);
        
        transform.localEulerAngles = new Vector3(newX, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    void ApplyMovement()
    {
        // 1. Convert the local target back to World Space based on current Camera transform
        Vector3 worldTargetPos = cam.transform.TransformPoint(localTargetPosition);

        // 2. Smoothly Lerp the object's world position to that world target
        transform.position = Vector3.Lerp(transform.position, worldTargetPos, Time.deltaTime * positionSpeed);
        
        // 3. Snap if close enough to stop calculations
        if (Vector3.Distance(transform.position, worldTargetPos) < 0.001f)
        {
            transform.position = worldTargetPos;
        }
    }
}