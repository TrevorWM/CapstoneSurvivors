using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private GameObject dungeonRoom;

    [SerializeField]
    private Tilemap doorTilemap;

    [SerializeField]
    private TileBase openDoorTile;

    [SerializeField]
    private bool roomComplete = false;

    private TileBase[] doorTiles = { };
    private int numberOfEnemies = 0;

    public void Awake()
    {
        //Tilemap doorTilemap = transform.GetComponent<Tilemap>();
    }

    public void Start()
    {
        GetDoorsInLevel();
    }

    public void OpenDoor()
    {
        Debug.Log("Room complete!");
        roomComplete = true;
        foreach (TileBase door in doorTiles)
        {
            doorTilemap.SwapTile(door, openDoorTile);
        }
    }

    private void GetDoorsInLevel()
    {
        if (doorTilemap)
        {
            Debug.Log("We have the Tilemap");

            foreach (Vector3Int position in doorTilemap.cellBounds.allPositionsWithin)
            {
                if (doorTilemap.HasTile(position))
                {
                    doorTiles.Append(doorTilemap.GetTile(position));
                    Debug.LogFormat("Found {0}!", doorTilemap.GetTile(position));
                }
            }
        }
    }

    public void AddEnemyToCount()
    {
        numberOfEnemies++;
    }

    public void RemoveEnemyFromCount()
    {
        numberOfEnemies--;
        if (numberOfEnemies <= 0) OpenDoor();
    }
}
