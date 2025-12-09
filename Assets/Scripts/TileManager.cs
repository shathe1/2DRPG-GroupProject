using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Tilemap tilemap;

    // Base Tile Assets (templates)
    public MemoryTileAsset safeTile;
    public MemoryTileAsset crackedTile;

    // Locked positions (must always be safe)
    public Vector3Int startTilePosition;
    public Vector3Int endTilePosition;

    // How long tiles stay revealed
    public float revealDuration = 2f;

    // Stores all tiles painted on the tilemap
    private List<Vector3Int> allTilePositions = new List<Vector3Int>();


    void Start()
    {
        CacheAllTilePositions();
        GenerateCrackedTiles();
        StartCoroutine(RevealTilesRoutine());
    }


    // -------------------------------------------------------
    // STEP 1 — Collect all tile positions from Tilemap
    // -------------------------------------------------------
    void CacheAllTilePositions()
    {
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                allTilePositions.Add(pos);
            }
        }
    }


    // -------------------------------------------------------
    // STEP 2 — Assign cracked or safe tiles correctly
    // -------------------------------------------------------
    void GenerateCrackedTiles()
    {
        foreach (var pos in allTilePositions)
        {
            MemoryTileAsset newTile;

            // Force SAFE tile on start & end
            if (pos == startTilePosition || pos == endTilePosition)
            {
                newTile = Instantiate(safeTile);
                newTile.isCracked = false;
                tilemap.SetTile(pos, newTile);
                continue;
            }

            // Random cracked chance
            bool cracked = Random.value < 0.35f;

            if (cracked)
            {
                newTile = Instantiate(crackedTile);
                newTile.isCracked = true;
            }
            else
            {
                newTile = Instantiate(safeTile);
                newTile.isCracked = false;
            }

            tilemap.SetTile(pos, newTile);
        }
    }


    // -------------------------------------------------------
    // STEP 3 — Reveal phase: shake + red flash
    // -------------------------------------------------------
    IEnumerator RevealTilesRoutine()
    {
        // Reveal ALL tiles visually
        foreach (Vector3Int pos in allTilePositions)
        {
            MemoryTileAsset tile = tilemap.GetTile<MemoryTileAsset>(pos);
            if (tile != null)
            {
                StartCoroutine(tile.RevealEffect(tilemap, pos));
            }
        }

        // Wait for player to memorize
        yield return new WaitForSeconds(revealDuration);

        // Reset all tile visuals
        foreach (var pos in allTilePositions)
        {
            MemoryTileAsset tile = tilemap.GetTile<MemoryTileAsset>(pos);
            if (tile != null)
            {
                tile.ResetVisual(tilemap, pos);
            }
        }
    }
}
