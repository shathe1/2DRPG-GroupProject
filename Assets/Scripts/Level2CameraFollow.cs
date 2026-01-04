using UnityEngine;

public class VerticalCameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Follow Settings")]
    public float smoothSpeed = 4f;
    public float minY = 0f; // starting camera Y

    private float targetY;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        targetY = transform.position.y;
        minY = targetY;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Follow player only if above current camera target
        float desiredY = Mathf.Max(targetY, player.position.y);

        Vector3 targetPos = new Vector3(
            transform.position.x,
            desiredY,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            smoothSpeed * Time.deltaTime
        );
    }

    // 🔑 CALLED BY SectionTrigger
    public void MoveCameraUp(float amount)
    {
        targetY += amount;
    }
}
