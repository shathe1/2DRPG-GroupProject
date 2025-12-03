using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isMoving = false;

    void Update()
    {
        // This will be replaced later with real movement
        isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
    }
}
