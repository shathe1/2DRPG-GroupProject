using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

[CreateAssetMenu(fileName = "MemoryTile", menuName = "Tiles/Memory Tile")]


public class MemoryTileAsset : Tile
{

    // Reveal animation (flash + shake)
    public IEnumerator RevealEffect(Tilemap tilemap, Vector3Int pos, bool cracked, float duration)
    {
        tilemap.SetTileFlags(pos, TileFlags.None);

        float timer = 0f;
        Color targetColor = cracked ? new Color(1f, 0.3f, 0.3f) : Color.white;
        float shake = cracked ? 0.12f : 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float flash = Mathf.PingPong(Time.time * 6f, 1f);
            tilemap.SetColor(pos, Color.Lerp(Color.white, targetColor, flash));

            if (cracked)
            {
                float x = Mathf.Sin(Time.time * 40f) * shake;
                float y = Mathf.Sin(Time.time * 45f) * shake;

                tilemap.SetTransformMatrix(
                    pos,
                    Matrix4x4.TRS(new Vector3(x, y, 0), Quaternion.identity, Vector3.one)
                );
            }

            yield return null;
        }

        tilemap.SetColor(pos, Color.white);
        tilemap.SetTransformMatrix(pos, Matrix4x4.identity);
    }

    public void ResetVisual(Tilemap tilemap, Vector3Int pos)
    {
        tilemap.SetColor(pos, Color.white);
        tilemap.SetTransformMatrix(pos, Matrix4x4.identity);
    }

    public IEnumerator StepOnCracked(Tilemap tilemap, Vector3Int pos)
    {
        tilemap.SetTileFlags(pos, TileFlags.None);

        float duration = 0.25f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float flash = Mathf.PingPong(Time.time * 10f, 1f);
            tilemap.SetColor(pos, Color.Lerp(Color.white, Color.red, flash));

            float x = Mathf.Sin(Time.time * 50f) * 0.1f;
            float y = Mathf.Sin(Time.time * 55f) * 0.1f;

            tilemap.SetTransformMatrix(
                pos,
                Matrix4x4.TRS(new Vector3(x, y, 0), Quaternion.identity, Vector3.one)
            );

            yield return null;
        }

        ResetVisual(tilemap, pos);
    }
}
