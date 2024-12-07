using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject PlayerObject;
    public float TileSize;
    public int NumTilesPerRow;
    public int NumTilesPerColumn;
    public int MapChosen;
    public GameObject[] Map1Tiles;
    public GameObject[] Map2Tiles;
    private LayerMask MapTileLayerMask;

    private Vector3 lastPlayerPosition;
    private Transform mapTileParent;

    void Start()
    {
        mapTileParent = GameObject.Find("MapTiles").transform;
        MapTileLayerMask = LayerMask.GetMask("MapTiles");
        GenerateInitialMap(ButtonManager.MapSelectNumber);
        lastPlayerPosition = Vector3.zero;
        MapChosen = ButtonManager.MapSelectNumber;
    }

    void Update()
    {
        if (GetComponent<GameManager>().CharChosen)
        {
            PlayerObject = GetComponent<GameManager>().PlayerObject;
            Vector3 currentPlayerPosition = PlayerObject.transform.position;

            // Check if the player has moved to a new tile
            if (Mathf.Abs(currentPlayerPosition.x - lastPlayerPosition.x) >= TileSize || Mathf.Abs(currentPlayerPosition.y - lastPlayerPosition.y) >= TileSize)
            {
                // Generate new map tiles based on player's movement
                GenerateNewMapTiles(currentPlayerPosition);
                lastPlayerPosition = currentPlayerPosition;
            }
        }
    }

    void GenerateInitialMap(int mapSelect)
    {
        // Calculate the position of the center tile
        Vector3 centerPosition = new Vector3((NumTilesPerRow / 2) * TileSize, (NumTilesPerColumn / 2) * TileSize, 0f);

        // Generate the initial map tiles around the center tile
        for (int i = 0; i < NumTilesPerRow; i++)
        {
            for (int j = 0; j < NumTilesPerColumn; j++)
            {
                Vector3 spawnPosition = new Vector3(i * TileSize, j * TileSize, 1f) - centerPosition;
                GameObject mapTilePrefab;
                switch (mapSelect)
                {
                    case 0:
                        int randomMapTile = Random.Range(0, Map1Tiles.Length);
                        mapTilePrefab = Map1Tiles[randomMapTile];
                        break;
                    case 1:
                        randomMapTile = Random.Range(0, Map2Tiles.Length);
                        mapTilePrefab = Map2Tiles[randomMapTile];
                        break;
                    default:
                        randomMapTile = Random.Range(0, Map1Tiles.Length);
                        mapTilePrefab = Map1Tiles[randomMapTile];
                        Debug.Log("Error finding map chosen, defaulting to map 1...");
                        break;
                }
                Instantiate(mapTilePrefab, spawnPosition, Quaternion.identity, mapTileParent);
            }
        }
    }

    void GenerateNewMapTiles(Vector3 direction)
    {
        Vector3 playerPosition = PlayerObject.transform.position;
        int playerTileIndexX = Mathf.RoundToInt(playerPosition.x / TileSize);
        int playerTileIndexY = Mathf.RoundToInt(playerPosition.y / TileSize);
        int playerTileIndexOffsetX = 0;
        int playerTileIndexOffsetY = 0;

        if (Mathf.Abs(playerTileIndexX) > 1)
        {
            playerTileIndexOffsetX = (int)Mathf.Sign(direction.x) * 2;
        }

        if (Mathf.Abs(playerTileIndexY) > 1)
        {
            playerTileIndexOffsetY = (int)Mathf.Sign(direction.y) * 2;
        }

        int bufferTiles = 2; // Number of buffer tiles around the camera view

        if (direction.x != 0)
        {
            int startX = playerTileIndexX - bufferTiles + playerTileIndexOffsetX;
            int endX = playerTileIndexX + bufferTiles + playerTileIndexOffsetX;
            for (int y = playerTileIndexY - bufferTiles; y <= playerTileIndexY + bufferTiles; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    Vector3 spawnPosition = new Vector3(x * TileSize, y * TileSize, 1f);
                    // Check if a tile already exists at the spawn position
                    if (!TileExistsAtPosition(spawnPosition))
                    {
                        GameObject mapTilePrefab;
                        switch (MapChosen)
                        {
                            case 0:
                                int randomMapTile = Random.Range(0, Map1Tiles.Length);
                                mapTilePrefab = Map1Tiles[randomMapTile];
                                break;
                            case 1:
                                randomMapTile = Random.Range(0, Map2Tiles.Length);
                                mapTilePrefab = Map2Tiles[randomMapTile];
                                break;
                            default:
                                randomMapTile = Random.Range(0, Map1Tiles.Length);
                                mapTilePrefab = Map1Tiles[randomMapTile];
                                Debug.Log("Error finding map chosen, defaulting to map 1...");
                                break;
                        }
                        Instantiate(mapTilePrefab, spawnPosition, Quaternion.identity, mapTileParent);
                    }
                }
            }
        }

        if (direction.y != 0)
        {
            int startY = playerTileIndexY - bufferTiles + playerTileIndexOffsetY;
            int endY = playerTileIndexY + bufferTiles + playerTileIndexOffsetY;
            for (int x = playerTileIndexX - bufferTiles; x <= playerTileIndexX + bufferTiles; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    Vector3 spawnPosition = new Vector3(x * TileSize, y * TileSize, 1f);
                    // Check if a tile already exists at the spawn position
                    if (!TileExistsAtPosition(spawnPosition))
                    {
                        GameObject mapTilePrefab;
                        switch (MapChosen)
                        {
                            case 0:
                                int randomMapTile = Random.Range(0, Map1Tiles.Length);
                                mapTilePrefab = Map1Tiles[randomMapTile];
                                break;
                            case 1:
                                randomMapTile = Random.Range(0, Map2Tiles.Length);
                                mapTilePrefab = Map2Tiles[randomMapTile];
                                break;
                            default:
                                randomMapTile = Random.Range(0, Map1Tiles.Length);
                                mapTilePrefab = Map1Tiles[randomMapTile];
                                Debug.Log("Error finding map chosen, defaulting to map 1...");
                                break;
                        }
                        Instantiate(mapTilePrefab, spawnPosition, Quaternion.identity, mapTileParent);
                    }
                }
            }
        }
    }

    bool TileExistsAtPosition(Vector3 position)
    {
        // Check if a tile exists at the specified position
        Collider2D hit = Physics2D.OverlapPoint(position, MapTileLayerMask);
        return hit != null;
    }

}