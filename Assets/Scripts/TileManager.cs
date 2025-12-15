using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Tilemap tilemap;

    public MemoryTileAsset safeTile;
    public MemoryTileAsset crackedTile;

    public float revealDuration = 2f;

    private PlatformData startPlatform;
    private PlatformData exitPlatform;

    private List<PlatformData> platforms = new List<PlatformData>();
    void CachePlatforms()
    {
        BoundsInt bounds = tilemap.cellBounds;
        HashSet<Vector3Int> used = new HashSet<Vector3Int>();

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (used.Contains(pos)) continue;
            if (!tilemap.HasTile(pos)) continue;

            Vector3Int right = pos + Vector3Int.right;
            if (!tilemap.HasTile(right)) continue;

            PlatformData p = new PlatformData
            {
                left = pos,
                right = right,
                isCracked = false
            };

            platforms.Add(p);
            used.Add(pos);
            used.Add(right);
        }
    }
    void GenerateCrackedPlatformsWithValidation()
    {
        // TEMP DEBUG: test BFS with ZERO cracked platforms
        foreach (var p in platforms)
            p.isCracked = false;

        if (!IsExitReachable())
        {
            Debug.LogError("BFS CANNOT reach exit even with NO cracked platforms!");
            return;
        }

        const int MAX_ATTEMPTS = 50;

        for (int attempt = 0; attempt < MAX_ATTEMPTS; attempt++)
        {
            foreach (var p in platforms)
                p.isCracked = false;

            List<PlatformData> candidates = new List<PlatformData>();
            foreach (var p in platforms)
            {
                if (p != startPlatform && p != exitPlatform)
                    candidates.Add(p);
            }

            candidates.Shuffle();

            for (int i = 0; i < 4 && i < candidates.Count; i++)
                candidates[i].isCracked = true;

            startPlatform.isCracked = false;
            exitPlatform.isCracked = false;

            if (IsExitReachable())
                return;
        }

        Debug.LogError("No valid platform layout found!");
    }
    int PlatformY(PlatformData p) => p.left.y;

    bool OverlapsX(PlatformData a, PlatformData b)
    {
        return a.left.x <= b.right.x && b.left.x <= a.right.x;
    }
    bool IsExitReachable()
    {
        Queue<PlatformData> queue = new Queue<PlatformData>();
        HashSet<PlatformData> visited = new HashSet<PlatformData>();

        if (startPlatform.isCracked)
            return false;

        queue.Enqueue(startPlatform);
        visited.Add(startPlatform);

        while (queue.Count > 0)
        {
            PlatformData current = queue.Dequeue();

            if (current == exitPlatform)
                return true;

            foreach (var p in platforms)
            {
                if (p.isCracked) continue;
                if (visited.Contains(p)) continue;

                int dy = PlatformY(p) - PlatformY(current);

                // Allow jump up to 3 tiles UP, unlimited fall
                bool canReachVertically =
                    (dy >= 0 && dy <= 3) ||   // jump up
                    (dy < 0);                // fall down

                if (canReachVertically && OverlapsX(current, p))
                {
                    visited.Add(p);
                    queue.Enqueue(p);
                }

            }
        }

        return false;
    }
    IEnumerator RevealAllPlatforms()
    {
        foreach (var p in platforms)
        {
            MemoryTileAsset tileA = tilemap.GetTile<MemoryTileAsset>(p.left);
            MemoryTileAsset tileB = tilemap.GetTile<MemoryTileAsset>(p.right);

            StartCoroutine(tileA.RevealEffect(tilemap, p.left, p.isCracked, revealDuration));
            StartCoroutine(tileB.RevealEffect(tilemap, p.right, p.isCracked, revealDuration));
        }

        yield return new WaitForSeconds(revealDuration);

        foreach (var p in platforms)
        {
            tilemap.GetTile<MemoryTileAsset>(p.left).ResetVisual(tilemap, p.left);
            tilemap.GetTile<MemoryTileAsset>(p.right).ResetVisual(tilemap, p.right);
        }
    }
    void Start()
    {
        CachePlatforms();
        AssignStartAndExitPlatforms();
        GenerateCrackedPlatformsWithValidation();
        ApplyPlatformVisuals();
        StartCoroutine(RevealAllPlatforms());
    }
    void ApplyPlatformVisuals()
    {
        foreach (var p in platforms)
        {
            MemoryTileAsset tileAsset = p.isCracked ? crackedTile : safeTile;

            tilemap.SetTile(p.left, Instantiate(tileAsset));
            tilemap.SetTile(p.right, Instantiate(tileAsset));

            tilemap.SetTileFlags(p.left, TileFlags.None);
            tilemap.SetTileFlags(p.right, TileFlags.None);
        }
    }
    public bool IsCrackedAt(Vector3Int cell)
    {
        foreach (var p in platforms)
        {
            if ((p.left == cell || p.right == cell) && p.isCracked)
                return true;
        }
        return false;
    }

    void AssignStartAndExitPlatforms()
    {
        startPlatform = null;
        exitPlatform = null;

        foreach (var p in platforms)
        {
            // Start = lowest Y platform
            if (startPlatform == null || p.left.y < startPlatform.left.y)
                startPlatform = p;

            // Exit = highest Y platform
            if (exitPlatform == null || p.left.y > exitPlatform.left.y)
                exitPlatform = p;
        }

        Debug.Assert(startPlatform != null, "Start platform not found!");
        Debug.Assert(exitPlatform != null, "Exit platform not found!");
    }


}


