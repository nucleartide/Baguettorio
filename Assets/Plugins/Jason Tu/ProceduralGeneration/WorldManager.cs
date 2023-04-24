using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private WorldConfiguration worldConfig;

    [SerializeField]
    private WorldDisplayMode worldDisplayMode;

    [SerializeField]
    [NotNull(IgnorePrefab = true)]
    private UnityEngine.Tilemaps.Tilemap tilemap;

    [SerializeField]
    [NotNull]
    private UnityEngine.Tilemaps.TileBase groundRuleTile;

    private WorldMap worldMap;

    private List<GameObject> tiles;

    private List<GameObject> InstantiateTiles()
    {
        var gridHeight = worldConfig.GridHeight;
        var gridWidth = worldConfig.GridWidth;
        var tiles = new List<GameObject>();

        for (var y = 0; y < gridHeight; y++)
        {
            for (var x = 0; x < gridWidth; x++)
            {
                var tile = worldMap.InstantiateTile(x, y, worldDisplayMode);
                tiles.Add(tile);
            }
        }

        return tiles;
    }

    private bool ShouldSetGroundTile(int x, int y)
    {
        var isWaterBiome = worldConfig.IsWaterBiome(worldMap.ClosestBiome(x, y));
        if (!isWaterBiome)
            return true;

        for (var i = -1; i <= 1; i++)
        {
            for (var j = -1; j <= 1; j++)
            {
                if (!worldConfig.IsWaterBiome(worldMap.ClosestBiome(x+i, y+j)))
                    return true;
            }
        }

        return false;
    }

    private void InstantiateTilemap()
    {
        var gridHeight = worldConfig.GridHeight;
        var gridWidth = worldConfig.GridWidth;

        for (var y = 0; y < gridHeight; y++)
        {
            for (var x = 0; x < gridWidth; x++)
            {
                if (ShouldSetGroundTile(x, y))
                {
                    tilemap.SetTile(new Vector3Int((int)(x - worldConfig.GridWidth * .5f), (int)(y - worldConfig.GridHeight * .5f), 0), groundRuleTile);
                }
            }
        }
    }

    public void RegenerateResources()
    {
        // Destroy if an existing set of tiles exists.
        if (tiles != null)
            foreach (var tile in tiles)
                Destroy(tile);

        // Construct a world map.
        worldMap = new WorldMap(worldConfig);

        // Instantiate the world map.
        tiles = InstantiateTiles();

        // Instantiate the tilemap as well.
        InstantiateTilemap();
    }

    public void OnEnable() => RegenerateResources();
}
