using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomLogic : MonoBehaviour, IDungeonRoom
{
    [SerializeField]
    private Transform playerStartPosition;

    [SerializeField]
    private Transform upgradeOrbPosition;

    [SerializeField]
    private Tilemap doorTilemap;

    [SerializeField]
    private TileBase openDoorTile;

    private List<TileBase> doorTiles = new List<TileBase>();

    private int enemiesInRoom = 0;
    private DoorLogic doorTilemapLogic;
    private RoomManager roomManager;
    private bool roomComplete = false;


    public void Start()
    {
        roomManager = FindObjectOfType<RoomManager>().GetComponent<RoomManager>();
        GetDoorsInLevel();
    }

    public Vector3 GetPlayerStartPosition() { return playerStartPosition.position; }
    public Vector3 GetUpgradeOrbPosition() { return upgradeOrbPosition.position; }

    /// <summary>
    /// Searches the tilemap with all the doors in order to get references to them for opening.
    /// Will also be used in the future if we decide to have branching paths.
    /// </summary>
    private void GetDoorsInLevel()
    {
        if (doorTilemap)
        {
            doorTilemapLogic = doorTilemap.GetComponent<DoorLogic>();

            foreach (Vector3Int position in doorTilemap.cellBounds.allPositionsWithin)
            {
                if (doorTilemap.HasTile(position))
                {
                    doorTiles.Add(doorTilemap.GetTile<TileBase>(position));
                }
            }
        }
    }

    /// <summary>
    /// Opens all of the doors in the current room and notifies the room manager
    /// and the doorTilemap 
    /// </summary>
    public void OpenDoor()
    {
        if (!roomComplete)
        {
            roomComplete = true;

            roomManager.OnRoomComplete();
            doorTilemapLogic.OnRoomComplete();

            foreach (TileBase door in doorTiles)
            {
                doorTilemap.SwapTile(door, openDoorTile);
            }
        }
        
    }

    public void RoomExited()
    {
        roomManager.GoToNextRoom();
    }

    public void EnemyAdded()
    {
        enemiesInRoom++;
    }

    public void EnemyRemoved()
    {
        enemiesInRoom--;

        if (enemiesInRoom <= 0) OpenDoor();
    }
}
