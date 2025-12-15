using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MovementType { Horizontal, Vertical }
    public MovementType movementType;

    public float speed = 2f;
    public float distance = 3f;

    private Vector3 startPos;
    private bool movingForward = true;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (movementType == MovementType.Horizontal)
            MoveHorizontal();
        else
            MoveVertical();
    }

    void MoveHorizontal()
    {
        float move = Mathf.PingPong(Time.time * speed, distance * 2) - distance;
        transform.position = new Vector3(startPos.x + move, startPos.y, startPos.z);
    }

    void MoveVertical()
    {
        float move = Mathf.PingPong(Time.time * speed, distance * 2) - distance;
        transform.position = new Vector3(startPos.x, startPos.y + move, startPos.z);
    }
}
