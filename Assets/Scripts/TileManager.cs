using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Tilemap tilemap;

    public MemoryTileAsset safeTile;
    public MemoryTileAsset crackedTile;

    // Positions refer to the LEFT cell of each 2-cell tile
    public Vector3Int startTileLeftPos;
    public Vector3Int endTileLeftPos;

    public float revealDuration = 2f;

    // Stores ONLY left-cell positions of tiles
    private List<Vector3Int> tileLeftPositions = new List<Vector3Int>();

    void Start()
    {
        CacheTileLeftPositions();
        GenerateTiles();
        StartCoroutine(RevealTilesRoutine());
    }


    // ----------------------------------------------------------
    // STEP 1 — Collect ONLY the left cell of each 2-cell tile
    // ----------------------------------------------------------
    void CacheTileLeftPositions()
    {
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            // Check if RIGHT neighbor exists → confirms it's a 2-cell tile
            Vector3Int rightPos = pos + new Vector3Int(1, 0, 0);

            if (tilemap.HasTile(rightPos))
            {
                tileLeftPositions.Add(pos);
            }
        }
    }


    // ----------------------------------------------------------
    // STEP 2 — Assign safe/cracked per full tile (2 cells)
    // ----------------------------------------------------------
    void GenerateTiles()
    {
        foreach (var leftPos in tileLeftPositions)
        {
            Vector3Int rightPos = leftPos + new Vector3Int(1, 0, 0);

            bool forceSafe = (leftPos == startTileLeftPos || leftPos == endTileLeftPos);

            bool cracked = !forceSafe && (Random.value < 0.35f);

            // Pick correct template
            MemoryTileAsset tileA = Instantiate(cracked ? crackedTile : safeTile);
            MemoryTileAsset tileB = Instantiate(cracked ? crackedTile : safeTile);

            tileA.isCracked = cracked;
            tileB.isCracked = cracked;

            // Apply to both squares
            tilemap.SetTile(leftPos, tileA);
            tilemap.SetTile(rightPos, tileB);
        }
    }


    // ----------------------------------------------------------
    // STEP 3 — Reveal effect for whole tiles (both squares)
    // ----------------------------------------------------------
    IEnumerator RevealTilesRoutine()
    {
        // Reveal
        foreach (var leftPos in tileLeftPositions)
        {
            Vector3Int rightPos = leftPos + new Vector3Int(1, 0, 0);

            MemoryTileAsset tileL = tilemap.GetTile<MemoryTileAsset>(leftPos);
            MemoryTileAsset tileR = tilemap.GetTile<MemoryTileAsset>(rightPos);

            if (tileL != null) StartCoroutine(tileL.RevealEffect(tilemap, leftPos));
            if (tileR != null) StartCoroutine(tileR.RevealEffect(tilemap, rightPos));
        }

        yield return new WaitForSeconds(revealDuration);

        // Reset
        foreach (var leftPos in tileLeftPositions)
        {
            Vector3Int rightPos = leftPos + new Vector3Int(1, 0, 0);

            MemoryTileAsset tileL = tilemap.GetTile<MemoryTileAsset>(leftPos);
            MemoryTileAsset tileR = tilemap.GetTile<MemoryTileAsset>(rightPos);

            if (tileL != null) tileL.ResetVisual(tilemap, leftPos);
            if (tileR != null) tileR.ResetVisual(tilemap, rightPos);
        }
    }
}
