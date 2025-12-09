using UnityEngine;

public class HazardMovement : MonoBehaviour
{
    public Vector3 pointA; // starting point
    public Vector3 pointB; // ending point
    public float speed = 2f; // movement speed

    private bool movingToB = true;

    void Start()
    {
        // Optional: initialize pointA to current position if left empty
        if (pointA == Vector3.zero)
            pointA = transform.position;
    }

    void Update()
    {
        if (movingToB)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointB, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, pointB) < 0.01f)
                movingToB = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pointA, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, pointA) < 0.01f)
                movingToB = true;
        }
    }
}
