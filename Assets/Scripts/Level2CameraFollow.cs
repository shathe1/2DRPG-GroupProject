using UnityEngine;

public class VerticalCameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private float lowestY;

    void Start()
    {
        lowestY = transform.position.y;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Only allow downward movement
        if (desiredPosition.y < lowestY)
            lowestY = desiredPosition.y;

        Vector3 finalPosition = new Vector3(
            desiredPosition.x,
            lowestY,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            finalPosition,
            smoothSpeed
        );
    }
}
