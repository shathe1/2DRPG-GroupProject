using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [Header("Tilemap")]
    public Tilemap tilemap;

    [Header("Tiles")]
    public MemoryTileAsset safeTile;
    public MemoryTileAsset crackedTile;

    [Header("Reveal Settings")]
    public float revealDuration = 2f;

    [Header("Manual Start / Exit")]
    public Vector3Int startCell;
    public Vector3Int exitCell;

    [Header("Audio")]
    public AudioClip crackedSound;
    public AudioClip revealSound;

    [Header("Section Role")]
    public bool useStartTile = false;
    public bool useExitTile = false;

    private PlatformData startPlatform;
    private PlatformData exitPlatform;

    private readonly List<PlatformData> platforms = new List<PlatformData>();
    private bool prepared = false;

    // ---------------- UNITY LIFECYCLE ----------------

    void Awake()
    {
        if (!tilemap)
            tilemap = GetComponent<Tilemap>();

        if (!tilemap)
            Debug.LogError($"{name}: Tilemap reference is MISSING");
    }

    void Start()
    {
        if (useStartTile)
        {
            PrepareSection();
            RevealSection();
        }
    }

    // ---------------- SECTION PREP ----------------

    public void PrepareSection()
    {
        if (prepared) return;

        platforms.Clear();

        CachePlatforms();
        AssignStartAndExitPlatforms();
        GenerateCrackedPlatformsWithValidation();
        ApplyPlatformVisuals();

        prepared = true;
        Debug.Log($"{name}: Section prepared with {platforms.Count} platforms");
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

            platforms.Add(new PlatformData
            {
                left = pos,
                right = right,
                isCracked = false
            });

            used.Add(pos);
            used.Add(right);
        }

        Debug.Log($"{name}: Cached {platforms.Count} platforms");
    }

    void AssignStartAndExitPlatforms()
    {
        startPlatform = null;
        exitPlatform = null;

        foreach (var p in platforms)
        {
            if (useStartTile && (p.left == startCell || p.right == startCell))
                startPlatform = p;

            if (useExitTile && (p.left == exitCell || p.right == exitCell))
                exitPlatform = p;
        }

        if (useStartTile && startPlatform == null)
            Debug.LogError($"{name}: START platform not found");

        if (useExitTile && exitPlatform == null)
            Debug.LogError($"{name}: EXIT platform not found");
    }

    // ---------------- REVEAL ----------------

    public void RevealSection()
    {
        StopAllCoroutines();
        StartCoroutine(RevealSectionRoutine());
    }

    IEnumerator RevealSectionRoutine()
    {
        if (platforms.Count == 0)
            yield break;

        if (revealSound && AudioManager.Instance)
            AudioManager.Instance.PlaySFX(revealSound);

        foreach (var p in platforms)
        {
            MemoryTileAsset tileA = tilemap.GetTile<MemoryTileAsset>(p.left);
            MemoryTileAsset tileB = tilemap.GetTile<MemoryTileAsset>(p.right);

            if (!tileA || !tileB)
                continue;

            StartCoroutine(tileA.RevealEffect(tilemap, p.left, p.isCracked, revealDuration));
            StartCoroutine(tileB.RevealEffect(tilemap, p.right, p.isCracked, revealDuration));
        }

        yield return new WaitForSeconds(revealDuration);

        foreach (var p in platforms)
        {
            tilemap.GetTile<MemoryTileAsset>(p.left)?.ResetVisual(tilemap, p.left);
            tilemap.GetTile<MemoryTileAsset>(p.right)?.ResetVisual(tilemap, p.right);
        }
    }

    // ---------------- PLATFORM LOGIC ----------------

    void GenerateCrackedPlatformsWithValidation()
    {
        const int MAX_ATTEMPTS = 50;

        for (int attempt = 0; attempt < MAX_ATTEMPTS; attempt++)
        {
            foreach (var p in platforms)
                p.isCracked = false;

            List<PlatformData> candidates = new List<PlatformData>();

            foreach (var p in platforms)
            {
                if (p == startPlatform || p == exitPlatform)
                    continue;

                candidates.Add(p);
            }

            candidates.Shuffle();

            int crackCount = Mathf.Min(5, candidates.Count);
            for (int i = 0; i < crackCount; i++)
                candidates[i].isCracked = true;

            if (IsLayoutValid())
                return;
        }
    }

    bool IsLayoutValid()
    {
        if (useStartTile && startPlatform == null)
            return false;

        foreach (var p in platforms)
        {
            if (p.isCracked) continue;

            bool hasMove = false;
            foreach (var other in platforms)
            {
                if (other == p) continue;
                if (IsNeighborReachable(p, other))
                {
                    hasMove = true;
                    break;
                }
            }

            if (!hasMove)
                return false;
        }

        return !useExitTile || HasSafeVerticalPath();
    }

    bool HasSafeVerticalPath()
    {
        if (startPlatform == null || exitPlatform == null)
            return true;

        HashSet<PlatformData> visited = new HashSet<PlatformData>();
        Queue<PlatformData> queue = new Queue<PlatformData>();

        queue.Enqueue(startPlatform);
        visited.Add(startPlatform);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current == exitPlatform)
                return true;

            foreach (var p in platforms)
            {
                if (p.isCracked || visited.Contains(p)) continue;
                if (!IsNeighborReachable(current, p)) continue;

                visited.Add(p);
                queue.Enqueue(p);
            }
        }

        return false;
    }

    bool IsNeighborReachable(PlatformData from, PlatformData to)
    {
        if (to.isCracked) return false;

        int dy = to.left.y - from.left.y;
        int dx = Mathf.Abs(((to.left.x + to.right.x) / 2) - ((from.left.x + from.right.x) / 2));

        bool verticalOK = (dy >= 0 && dy <= 3) || dy < 0;
        bool horizontalOK = dx <= 3;

        return verticalOK && horizontalOK;
    }

    // ---------------- PUBLIC API ----------------

    public bool IsCrackedAt(Vector3Int cell)
    {
        foreach (var p in platforms)
            if ((p.left == cell || p.right == cell) && p.isCracked)
                return true;

        return false;
    }

    public PlatformData GetPlatformAt(Vector3Int cell)
    {
        foreach (var p in platforms)
            if (p.left == cell || p.right == cell)
                return p;

        return null;
    }

    public IEnumerator PlayCrackedPlatformEffect(PlatformData platform)
    {
        if (platform == null)
            yield break;

        if (crackedSound && AudioManager.Instance)
            AudioManager.Instance.PlaySFX(crackedSound);

        MemoryTileAsset tileA = tilemap.GetTile<MemoryTileAsset>(platform.left);
        MemoryTileAsset tileB = tilemap.GetTile<MemoryTileAsset>(platform.right);

        if (!tileA || !tileB)
            yield break;

        yield return StartCoroutine(tileA.StepOnCracked(tilemap, platform.left));
        yield return StartCoroutine(tileB.StepOnCracked(tilemap, platform.right));
    }

    public bool IsExitCell(Vector3Int cell) => cell == exitCell;
}
