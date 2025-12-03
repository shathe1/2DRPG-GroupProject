using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public List<MemoryTile> allTiles = new List<MemoryTile>();
    public int crackedTileCount = 5;
    public float revealDuration = 2f;

    void Start()
    {
        RandomizeCrackedTiles();
        StartCoroutine(RevealCrackedTiles());
    }

    void RandomizeCrackedTiles()
    {
        // Mark all tiles safe first
        foreach (var tile in allTiles)
        {
            tile.isCracked = false;
        }

        // Tile 1 ALWAYS safe
        if (allTiles.Count > 0)
            allTiles[0].isCracked = false;

        // Prepare list of possible cracked tiles
        List<int> available = new List<int>();
        for (int i = 1; i < allTiles.Count; i++) // start at 1 to exclude tile 1
            available.Add(i);

        // Randomly assign cracked tiles
        for (int i = 0; i < crackedTileCount; i++)
        {
            int rand = Random.Range(0, available.Count);
            int index = available[rand];
            available.RemoveAt(rand);

            allTiles[index].isCracked = true;
        }
    }

    IEnumerator RevealCrackedTiles()
    {
        // Make cracked tiles shake during reveal
        foreach (var tile in allTiles)
        {
            if (tile.isCracked)
                tile.RevealShake(revealDuration);
        }

        // Wait for reveal to end
        yield return new WaitForSeconds(revealDuration);

        // After reveal → no visual but cracked tiles remain deadly
        Debug.Log("Reveal finished. Tiles now look normal but cracked tiles are active.");
    }
}
