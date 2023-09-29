using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startingRoom;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject upgradeOrb;

    [SerializeField]
    private int roomsBeforeBoss;

    [SerializeField, SerializeReference]
    private GameObject[] tutorialRoomPool;

    [SerializeField, SerializeReference]
    private GameObject[] roomPool;

    [SerializeField, SerializeReference]
    private GameObject[] bossRoomPool;

    private GameObject currentRoom;
    private IDungeonRoom currentRoomLogic;
    private GameObject nextRoom;

    private GameObject currentPlayer;
    private GameObject currentUpgradeOrb;

    private int roomCount;


    private void OnValidate()
    {
        roomsBeforeBoss = Mathf.Max(1, roomsBeforeBoss);
    }

    public void Awake()
    {
        roomCount = 0;
        StartRoom(startingRoom);
    }

    /// <summary>
    /// This function checks if there is a current room, and destroys it if it exists.
    /// Afterwards it will create an instance of the room prefab that we will be entering.
    /// It will create a new instance of the player if this is the first room, but if not then
    /// it will simply move the player to the new room start position definied in the room prefab.
    /// </summary>
    /// <param name="newRoom"></param>
    public void StartRoom(GameObject newRoom)
    {
        if (currentRoom) Destroy(currentRoom);

        currentRoom = Instantiate<GameObject>(newRoom);
        currentRoomLogic = currentRoom.GetComponent<IDungeonRoom>();

        Vector3 playerPosition = currentRoomLogic.GetPlayerStartPosition();

        if (currentPlayer == null)
        {
            currentPlayer = Instantiate(player, playerPosition, Quaternion.identity);
            CharacterStats playerStats = currentPlayer.GetComponent<CharacterStats>();
            playerStats.playerDied.AddListener(RestartGame);

        } else
        {
            currentPlayer.transform.position = playerPosition;
        }
    }

    /// <summary>
    /// Called by the RoomLogic script for the individual level. This will increase our roomCount
    /// so that we can keep track of when the next room should be a boss instead of a random level.
    /// It will then pick a random room from the pool of rooms assigned in the inspector. This also
    /// creates an instance of the Upgrade Orb if it does not exist yet, or simply moves it if it does.
    /// </summary>
    public void OnRoomComplete()
    {
        Debug.Log("Room Complete!");
        roomCount++;

        nextRoom = ChooseNextRoom(roomCount);

        Vector3 upgradeOrbPosition = currentRoomLogic.GetUpgradeOrbPosition();

        if (currentUpgradeOrb == null)
        {
            currentUpgradeOrb = Instantiate(upgradeOrb, upgradeOrbPosition, Quaternion.identity);
            currentUpgradeOrb.GetComponent<UpgradeOrb>().InitializeOrb(currentPlayer);
        }
        else
        {
            currentUpgradeOrb.transform.position = upgradeOrbPosition;
            currentUpgradeOrb.SetActive(true);
        }
    }

    /// <summary>
    /// Called when the player actually walks to a door. This loads in the next room, and
    /// sets the Upgrade Orb to inactive so that the player doesn't see it upon loading into the
    /// room right away.
    /// </summary>
    public void GoToNextRoom()
    {
        StartRoom(nextRoom);
        currentUpgradeOrb?.SetActive(false);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private GameObject ChooseNextRoom(int roomCount)
    {
        GameObject nextRoom = null;
        int roomIndex;


        if (roomCount % roomsBeforeBoss == 0)
        {
            roomIndex = UnityEngine.Random.Range(0, bossRoomPool.Length);
            nextRoom = bossRoomPool[roomIndex];
        }
        else if (roomCount > 2)
        {
            roomIndex = UnityEngine.Random.Range(0, roomPool.Length);
            nextRoom = roomPool[roomIndex];
        }
        else
        {
            if (tutorialRoomPool != null) nextRoom = tutorialRoomPool[roomCount - 1];
        }

        return nextRoom;
    }
}
