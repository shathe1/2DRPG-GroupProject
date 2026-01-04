using System.Collections;
using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public float cameraMoveAmount = 6f;
    public float revealDelay = 2f;

    [Header("EXACT TileManager for this section")]
    public TileManager tileManager;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        if (tileManager == null)
        {
            Debug.LogError($"{name}: TileManager NOT ASSIGNED");
            return;
        }

        triggered = true;

        Debug.Log($"{name} triggering {tileManager.gameObject.name}");

        tileManager.PrepareSection();
        StartCoroutine(HandleSection());
        Debug.Log("SECTION 2 TRIGGER HIT");

    }

    private IEnumerator HandleSection()
    {
        // 1️⃣ Move camera up
        VerticalCameraFollow cam =
            Camera.main.GetComponent<VerticalCameraFollow>();

        if (cam != null)
        {
            cam.MoveCameraUp(cameraMoveAmount);
        }

        // 2️⃣ Wait for camera to settle
        yield return new WaitForSeconds(revealDelay);

        // 3️⃣ PREPARE section (generate cracked layout)
        tileManager.PrepareSection();

        // 4️⃣ REVEAL cracked tiles
        tileManager.RevealSection();

        // 5️⃣ Disable trigger forever
        GetComponent<Collider2D>().enabled = false;
    }

}
