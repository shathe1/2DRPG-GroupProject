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

    [Header("Manual Start / Exit")]
    public Vector3Int startCell;
    public Vector3Int exitCell;

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
        const int MAX_ATTEMPTS = 50;

        for (int attempt = 0; attempt < MAX_ATTEMPTS; attempt++)
        {
            // Reset
            foreach (var p in platforms)
                p.isCracked = false;

            // Collect crackable platforms
            List<PlatformData> candidates = new List<PlatformData>();

            foreach (var p in platforms)
            {
                if (p == startPlatform) continue;
                if (p == exitPlatform) continue;
                candidates.Add(p);
            }

            candidates.Shuffle();

            int crackCount = Mathf.Min(4, candidates.Count);
            for (int i = 0; i < crackCount; i++)
                candidates[i].isCracked = true;

            // 🔑 THIS IS THE IMPORTANT CHECK
            if (IsLayoutValid())
                return;
        }

        Debug.LogError("No valid platform layout found!");
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
            if (p.left == startCell || p.right == startCell)
                startPlatform = p;

            if (p.left == exitCell || p.right == exitCell)
                exitPlatform = p;
        }

        if (startPlatform == null)
            Debug.LogError("START platform not found! Check startCell.");

        if (exitPlatform == null)
            Debug.LogError("EXIT platform not found! Check exitCell.");
    }

    bool IsProtectedPlatform(PlatformData p)
    {
        return p == startPlatform || p == exitPlatform;
    }

    bool IsNeighborReachable(PlatformData from, PlatformData to)
    {
        if (to.isCracked) return false;

        int dy = to.left.y - from.left.y;

        int fromCenterX = (from.left.x + from.right.x) / 2;
        int toCenterX   = (to.left.x + to.right.x) / 2;

        int dx = Mathf.Abs(toCenterX - fromCenterX);

        bool verticalOK =
            (dy >= 0 && dy <= 3) ||   // jump up
            (dy < 0);                // fall down

        bool horizontalOK = dx <= 3;  // tune if needed

        return verticalOK && horizontalOK;
    }

    bool IsLayoutValid()
    {
        // Rule 1 & 2: No safe platform is a dead end
        foreach (var p in platforms)
        {
            if (p.isCracked) continue;

            bool hasSafeMove = false;

            foreach (var other in platforms)
            {
                if (other == p) continue;

                if (IsNeighborReachable(p, other))
                {
                    hasSafeMove = true;
                    break;
                }
            }

            if (!hasSafeMove)
                return false;
        }

        // Rule 3: Must be able to climb upward safely
        return HasSafeVerticalPath();
    }

    bool HasSafeVerticalPath()
    {
        HashSet<PlatformData> reachable = new HashSet<PlatformData>();
        Queue<PlatformData> queue = new Queue<PlatformData>();

        reachable.Add(startPlatform);
        queue.Enqueue(startPlatform);

        while (queue.Count > 0)
        {
            PlatformData current = queue.Dequeue();

            // If we've reached exit row or exit platform → success
            if (current == exitPlatform)
                return true;

            foreach (var p in platforms)
            {
                if (p.isCracked) continue;
                if (reachable.Contains(p)) continue;

                if (IsNeighborReachable(current, p))
                {
                    reachable.Add(p);
                    queue.Enqueue(p);
                }
            }
        }

        return false;
    }
    public PlatformData GetPlatformAt(Vector3Int cell)
    {
        foreach (var p in platforms)
        {
            if (p.left == cell || p.right == cell)
                return p;
        }
        return null;
    }

    public IEnumerator PlayCrackedPlatformEffect(PlatformData platform)
    {
        if (platform == null)
            yield break;

        MemoryTileAsset tileA = tilemap.GetTile<MemoryTileAsset>(platform.left);
        MemoryTileAsset tileB = tilemap.GetTile<MemoryTileAsset>(platform.right);

        if (tileA == null || tileB == null)
            yield break;

        // Play both tile effects in parallel
        Coroutine a = StartCoroutine(
            tileA.StepOnCracked(tilemap, platform.left)
        );

        Coroutine b = StartCoroutine(
            tileB.StepOnCracked(tilemap, platform.right)
        );

        yield return a;
        yield return b;
    }
}


