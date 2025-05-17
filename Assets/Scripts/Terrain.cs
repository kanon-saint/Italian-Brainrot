using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField] private Transform player;  // Player transform
    [SerializeField] private GameObject chunkPrefab;  // Chunk prefab to instantiate
    [SerializeField] private GameObject treePrefab;  // Tree prefab to instantiate
    [SerializeField] private GameObject grassPrefab;  // Grass prefab to instantiate
    [SerializeField] private GameObject rockPrefab;  // Rock prefab to instantiate

    [SerializeField] private int chunkSize = 100;  // Size of each chunk
    [SerializeField] private int generationRange = 2;  // How many chunks around the player to generate
    [SerializeField] private float unloadDistance = 150f;  // Distance at which chunks should be unloaded

    [SerializeField] private float treeDensity = 0.01f;  // Reduced density of trees (percentage of area)
    [SerializeField] private float grassDensity = 0.05f;  // Reduced density of grass
    [SerializeField] private float rockDensity = 0.02f;  // Reduced density of rocks

    private HashSet<Vector2Int> generatedChunks = new HashSet<Vector2Int>();  // To keep track of all loaded chunks
    private List<GameObject> activeChunks = new List<GameObject>();  // Store active chunks
    private HashSet<Vector2Int> unloadedChunks = new HashSet<Vector2Int>();  // Store unloaded chunks

    // Store the positions of environmental objects per chunk
    private Dictionary<Vector2Int, List<Vector3>> treePositions = new Dictionary<Vector2Int, List<Vector3>>();
    private Dictionary<Vector2Int, List<Vector3>> grassPositions = new Dictionary<Vector2Int, List<Vector3>>();
    private Dictionary<Vector2Int, List<Vector3>> rockPositions = new Dictionary<Vector2Int, List<Vector3>>();

    private Vector2Int lastPlayerChunkCoord;

    void Start()
    {
        lastPlayerChunkCoord = GetChunkCoord(player.position);
        GenerateChunksAroundPlayer();
    }

    void Update()
    {
        Vector2Int currentChunkCoord = GetChunkCoord(player.position);

        // If the player has moved to a new chunk, generate more and unload distant chunks
        if (currentChunkCoord != lastPlayerChunkCoord)
        {
            lastPlayerChunkCoord = currentChunkCoord;
            GenerateChunksAroundPlayer();
            UnloadFarChunks();
            ReloadUnloadedChunks();
        }
    }

    Vector2Int GetChunkCoord(Vector3 position)
    {
        return new Vector2Int(
            Mathf.FloorToInt(position.x / chunkSize),
            Mathf.FloorToInt(position.z / chunkSize)
        );
    }

    void GenerateChunksAroundPlayer()
    {
        Vector2Int playerChunkCoord = GetChunkCoord(player.position);

        // Loop through the grid of chunks to generate
        for (int x = -generationRange; x <= generationRange; x++)
        {
            for (int z = -generationRange; z <= generationRange; z++)
            {
                Vector2Int chunkCoord = playerChunkCoord + new Vector2Int(x, z);

                if (!generatedChunks.Contains(chunkCoord))
                {
                    // Create new chunk at the calculated position
                    Vector3 chunkPosition = new Vector3(chunkCoord.x * chunkSize, 0, chunkCoord.y * chunkSize);
                    GameObject chunk = Instantiate(chunkPrefab, chunkPosition, Quaternion.identity);
                    activeChunks.Add(chunk);  // Add it to the active chunks list
                    generatedChunks.Add(chunkCoord);  // Mark this chunk as generated

                    // Now add environment elements (trees, grass, rocks)
                    AddEnvironmentElements(chunkCoord, chunkPosition);
                }
            }
        }
    }

    void AddEnvironmentElements(Vector2Int chunkCoord, Vector3 chunkPosition)
    {
        // Instantiate environmental objects with densities

        // Trees
        int treeCount = Mathf.RoundToInt(chunkSize * chunkSize * treeDensity);
        List<Vector3> treeList = new List<Vector3>();  // List to store positions of trees
        for (int i = 0; i < treeCount; i++)
        {
            Vector3 treePosition = new Vector3(
                chunkPosition.x + Random.Range(0, chunkSize),
                0,
                chunkPosition.z + Random.Range(0, chunkSize)
            );
            treeList.Add(treePosition);
            Instantiate(treePrefab, treePosition, Quaternion.identity);
        }
        treePositions[chunkCoord] = treeList;  // Save positions of trees

        // Grass
        int grassCount = Mathf.RoundToInt(chunkSize * chunkSize * grassDensity);
        List<Vector3> grassList = new List<Vector3>();  // List to store positions of grass
        for (int i = 0; i < grassCount; i++)
        {
            Vector3 grassPosition = new Vector3(
                chunkPosition.x + Random.Range(0, chunkSize),
                0,
                chunkPosition.z + Random.Range(0, chunkSize)
            );
            grassList.Add(grassPosition);
            Instantiate(grassPrefab, grassPosition, Quaternion.identity);
        }
        grassPositions[chunkCoord] = grassList;  // Save positions of grass

        // Rocks
        int rockCount = Mathf.RoundToInt(chunkSize * chunkSize * rockDensity);
        List<Vector3> rockList = new List<Vector3>();  // List to store positions of rocks
        for (int i = 0; i < rockCount; i++)
        {
            Vector3 rockPosition = new Vector3(
                chunkPosition.x + Random.Range(0, chunkSize),
                0,
                chunkPosition.z + Random.Range(0, chunkSize)
            );
            rockList.Add(rockPosition);
            Instantiate(rockPrefab, rockPosition, Quaternion.identity);
        }
        rockPositions[chunkCoord] = rockList;  // Save positions of rocks
    }

    void UnloadFarChunks()
    {
        Vector2Int playerChunkCoord = GetChunkCoord(player.position);

        // Loop through all active chunks to check their distance from the player
        List<GameObject> chunksToUnload = new List<GameObject>();

        foreach (GameObject chunk in activeChunks)
        {
            Vector3 chunkPosition = chunk.transform.position;
            float distanceToPlayer = Vector3.Distance(player.position, chunkPosition);

            // If the chunk is far away from the player, add it to unload list
            if (distanceToPlayer > unloadDistance)
            {
                chunksToUnload.Add(chunk);
            }
        }

        // Unload the far chunks
        foreach (GameObject chunk in chunksToUnload)
        {
            activeChunks.Remove(chunk);
            generatedChunks.Remove(GetChunkCoord(chunk.transform.position));
            unloadedChunks.Add(GetChunkCoord(chunk.transform.position));  // Add chunk to unloaded chunks
            Destroy(chunk);  // Destroy the chunk GameObject

            // Remove environmental objects data for this chunk
            Vector2Int chunkCoord = GetChunkCoord(chunk.transform.position);
            treePositions.Remove(chunkCoord);  // Remove tree positions data
            grassPositions.Remove(chunkCoord);  // Remove grass positions data
            rockPositions.Remove(chunkCoord);  // Remove rock positions data
        }
    }

    void ReloadUnloadedChunks()
    {
        Vector2Int playerChunkCoord = GetChunkCoord(player.position);

        // Check if any unloaded chunk is now within range of the player
        foreach (var unloadedChunkCoord in unloadedChunks)
        {
            if (Vector2Int.Distance(playerChunkCoord, unloadedChunkCoord) <= generationRange)
            {
                // Instantiate the chunk again at the correct position
                Vector3 chunkPosition = new Vector3(unloadedChunkCoord.x * chunkSize, 0, unloadedChunkCoord.y * chunkSize);
                GameObject chunk = Instantiate(chunkPrefab, chunkPosition, Quaternion.identity);
                activeChunks.Add(chunk);  // Add it to the active chunks list
                generatedChunks.Add(unloadedChunkCoord);  // Mark this chunk as generated
                unloadedChunks.Remove(unloadedChunkCoord);  // Remove from unloaded list

                // Add environment elements back to the chunk based on saved positions
                if (treePositions.ContainsKey(unloadedChunkCoord))
                {
                    foreach (Vector3 pos in treePositions[unloadedChunkCoord])
                    {
                        Instantiate(treePrefab, pos, Quaternion.identity);
                    }
                }

                if (grassPositions.ContainsKey(unloadedChunkCoord))
                {
                    foreach (Vector3 pos in grassPositions[unloadedChunkCoord])
                    {
                        Instantiate(grassPrefab, pos, Quaternion.identity);
                    }
                }

                if (rockPositions.ContainsKey(unloadedChunkCoord))
                {
                    foreach (Vector3 pos in rockPositions[unloadedChunkCoord])
                    {
                        Instantiate(rockPrefab, pos, Quaternion.identity);
                    }
                }
            }
        }
    }
}