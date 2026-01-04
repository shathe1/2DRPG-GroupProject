using UnityEngine;

public class VerticalCameraFollow : MonoBehaviour
{
    public float smoothSpeed = 0.15f;

    private float targetY;
    private bool isMoving = false;

    void Start()
    {
        // Lock initial camera position
        targetY = transform.position.y;
    }

    void LateUpdate()
    {
        if (!isMoving) return;

        Vector3 targetPosition = new Vector3(
            transform.position.x,
            targetY,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed
        );

        // Stop movement when close enough
        if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
        {
            transform.position = targetPosition;
            isMoving = false;
        }
    }

    // 🔥 This is what SectionTriggers will call
    public void MoveCameraUp(float amount)
    {
        if (isMoving) return;

        targetY += amount;
        isMoving = true;
    }
}
