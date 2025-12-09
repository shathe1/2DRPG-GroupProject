using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

[CreateAssetMenu(fileName = "MemoryTile", menuName = "Tiles/Memory Tile")]
public class MemoryTileAsset : Tile
{
    public bool isCracked = false;
    public Sprite safeSprite;
    public Sprite crackedSprite;


    // ----------------------------
    // Reveal the actual sprite (cracked or safe)
    // ----------------------------
    public void RevealShake(Tilemap tilemap, Vector3Int position)
    {
        // Assign the correct sprite to THIS tile
        if (isCracked)
            this.sprite = crackedSprite;
        else
            this.sprite = safeSprite;

        // Re-apply tile so tilemap uses new sprite
        tilemap.SetTile(position, this);
    }


    // ----------------------------
    // Reset sprite to safe (hidden state)
    // ----------------------------
    public void ResetVisual(Tilemap tilemap, Vector3Int position)
    {
        this.sprite = safeSprite;   // Always appear safe when hidden
        tilemap.SetTile(position, this);
    }

    public IEnumerator RevealEffect(Tilemap tilemap, Vector3Int position)
    {
        float revealDuration = 3f;
        float timer = 0f;

        // Colors for safe vs cracked reveal
        Color startColor = Color.white;
        Color flashColor = isCracked ? new Color(1f, 0.2f, 0.2f) : new Color(1f, 1f, 1f);

        // Slight shaking values
        float shakeStrength = isCracked ? 0.12f : 0.05f;

        while (timer < revealDuration)
        {
            timer += Time.deltaTime;

            // --- FLASH COLOR ---
            float flash = Mathf.PingPong(Time.time * 6f, 1f);
            Color lerpedColor = Color.Lerp(startColor, flashColor, flash);
            tilemap.SetColor(position, lerpedColor);

            // --- VIBRATION / SHAKE (scale simulation) ---
            float shakeX = Mathf.Sin(Time.time * 40f) * shakeStrength;
            float shakeY = Mathf.Sin(Time.time * 45f) * shakeStrength;

            tilemap.SetTransformMatrix(position, Matrix4x4.TRS(
                new Vector3(shakeX, shakeY, 0),
                Quaternion.identity,
                Vector3.one
            ));

            yield return null;
        }

        // RESET color and transform
        tilemap.SetColor(position, Color.white);
        tilemap.SetTransformMatrix(position, Matrix4x4.identity);
    }
    public IEnumerator StepOnCracked(Tilemap tilemap, Vector3Int pos)
    {
        if (!isCracked) yield break;

        float duration = 0.5f;
        float elapsed = 0f;
        Color originalColor = tilemap.GetColor(pos);

        while (elapsed < duration)
        {
            float t = Mathf.PingPong(elapsed * 8f, 1f);

            tilemap.SetColor(pos, Color.Lerp(originalColor, Color.red, t));

            Vector3 offset = new Vector3(
                Random.Range(-0.05f, 0.05f),
                Random.Range(-0.05f, 0.05f),
                0);

            tilemap.SetTransformMatrix(pos, Matrix4x4.TRS(offset, Quaternion.identity, Vector3.one));

            elapsed += Time.deltaTime;
            yield return null;
        }

        tilemap.SetColor(pos, originalColor);
        tilemap.SetTransformMatrix(pos, Matrix4x4.identity);
    }


}
