using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "MemoryTile", menuName = "Tiles/Memory Tile")]
public class MemoryTileAsset : Tile
{
    public bool isCracked = false;
    public Sprite safeSprite;
    public Sprite crackedSprite;
    // Start is called before the first frame update
    
}
