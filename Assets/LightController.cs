using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    public SpriteRenderer floor; // Reference to the floor sprite
    public PlayerMovement player; // Reference to the player movement script

    public float greenTime = 3f; 
    public float redTime = 2f;  

    bool isRed = false;

    void Start()
    {
        StartCoroutine(LightCycle());
    }

    IEnumerator LightCycle()
    {
        while (true)
        {
            // GREEN PHASE
            isRed = false;
            floor.color = new Color(0f, 2f, 0f, 1f);
            yield return new WaitForSeconds(greenTime);

            // RED PHASE
            isRed = true;
            floor.color = new Color(2f, 0f, 0f, 1f);
            yield return new WaitForSeconds(redTime);
        }
    }

    void Update()
    {
        if (isRed && player.isMoving)
        {
            // Player moved during red!!!!
            Debug.Log("YOU MOVED DURING RED → DIE!");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
        }
    }
}
