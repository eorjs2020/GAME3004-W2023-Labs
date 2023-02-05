using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Header("Maze Properties")]
    public int width = 5;
    public int depth = 5;
    public List<GameObject> tileList;

    private Transform tileParent;
    private GameObject startTile;
    private GameObject goalTile;
    private GameObject[] tileArray;
    private GameObject playerPrefab;
    private GameObject player;

    void Awake()
    {
        startTile = Resources.Load<GameObject>("Prefabs/Start Tile");
        goalTile = Resources.Load<GameObject>("Prefabs/Goal Tile");

        tileArray = new GameObject[2];
        tileArray[0] = Resources.Load<GameObject>("Prefabs/Tile1");
        tileArray[1] = Resources.Load<GameObject>("Prefabs/Tile2");

        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");

        tileList = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        tileParent = GameObject.Find("[TILES]").transform;

        BuildTileList();
    }

    private void BuildTileList()
    {
        for (var row = 0; row < depth; row++)
        {
            for (var col = 0; col < width; col++)
            {
                var tempTile = Instantiate(tileArray[Random.Range(0, 2)], new Vector3(col * 16.0f, 0.0f,  row * 16.0f), 
                    Quaternion.Euler(0.0f, Random.Range(1, 4) * 90.0f, 0.0f), tileParent);
                tileList.Add(tempTile);
            }
        }

        // Set the Start Tile
        var StartTileIndex = Random.Range(0, width);
        var StartTilePosition = tileList[StartTileIndex].transform.position;
        Destroy(tileList[StartTileIndex]);
        tileList[StartTileIndex] = Instantiate(startTile, StartTilePosition, Quaternion.identity, tileParent);
        AddPlayer(tileList[StartTileIndex].transform.position);

        // Set the Goal Tile
        var GoalTileIndex = (width * depth) - Random.Range(0, width) - 1;
        var GoalTilePosition = tileList[GoalTileIndex].transform.position;
        Destroy(tileList[GoalTileIndex]);
        tileList[GoalTileIndex] = Instantiate(goalTile, GoalTilePosition, Quaternion.identity, tileParent);
    }

    private void AddPlayer(Vector3 position)
    {
        player = Instantiate(playerPrefab, new Vector3(position.x, 10.0f, position.z), Quaternion.identity);
    }

}
