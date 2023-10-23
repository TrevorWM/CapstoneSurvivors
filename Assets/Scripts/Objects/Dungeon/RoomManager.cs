using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
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
    private GameObject[] level2RoomPool;

    [SerializeField, SerializeReference]
    private GameObject[] bossRoomPool;

    [SerializeField, SerializeReference]
    private GameObject[] BonusRoomPool;

    private GameObject currentRoom;
    private IDungeonRoom currentRoomLogic;
    private GameObject nextRoom;

    private GameObject currentPlayer;
    private GameObject currentUpgradeOrb;
    private UpgradeOrb upgradeOrbLogic;

    private int roomCount;
    private int floorCount;
    private int treasureRoomCount;
    public GameObject CurrentRoom { get => currentRoom; }

    private void OnValidate()
    {
        roomsBeforeBoss = Mathf.Max(3, roomsBeforeBoss);
    }

    public void Awake()
    {
        roomCount = 0;
        floorCount = 0;
        treasureRoomCount = roomsBeforeBoss + 1;
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
            currentPlayer.GetComponent<PlayerControls>().ReleaseAllProjectiles();
            TreasureRoomLogic();
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

        nextRoom = ChooseNextRoom();

        Vector3 upgradeOrbPosition = currentRoomLogic.GetUpgradeOrbPosition();

        if (currentUpgradeOrb == null)
        {
            currentUpgradeOrb = Instantiate(upgradeOrb, upgradeOrbPosition, Quaternion.identity);
            upgradeOrbLogic = currentUpgradeOrb.GetComponent<UpgradeOrb>();
            upgradeOrbLogic.InitializeOrb(currentPlayer);
            
        }
        else
        {
            currentUpgradeOrb.transform.position = upgradeOrbPosition;
            currentUpgradeOrb.SetActive(true);
        }
        upgradeOrbLogic.orbUsed.AddListener(currentRoomLogic.OpenDoor);
    }

    /// <summary>
    /// Called when the player actually walks to a door. This loads in the next room, and
    /// sets the Upgrade Orb to inactive so that the player doesn't see it upon loading into the
    /// room right away.
    /// </summary>
    public void GoToNextRoom()
    {
        SoundEffectPlayer.Instance.EnterDoorSound();
        upgradeOrbLogic.orbUsed.RemoveListener(currentRoomLogic.OpenDoor);
        StartRoom(nextRoom);
        currentUpgradeOrb?.SetActive(false);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private GameObject ChooseNextRoom()
    {
        GameObject nextRoom = null;
        int roomIndex;

        if (roomCount % (treasureRoomCount) == 0)
        {
            roomIndex = 0;
            nextRoom = BonusRoomPool[roomIndex];
        }
        else if (roomCount % roomsBeforeBoss == 0)
        {
            roomIndex = floorCount;
            nextRoom = bossRoomPool[Mathf.Min(0, roomIndex)];
        }
        else if (roomCount > 2 && floorCount % 2 == 1)
        {
            roomIndex = UnityEngine.Random.Range(0, roomPool.Length);
            nextRoom = roomPool[roomIndex];
        }
        else if (floorCount % 2 == 1)
        {
            roomIndex = UnityEngine.Random.Range(0, level2RoomPool.Length);
            nextRoom = level2RoomPool[roomIndex];
        }
        else
        {
            if (tutorialRoomPool != null) nextRoom = tutorialRoomPool[roomCount - 1];
        }

        return nextRoom;
    }

    /// <summary>
    /// Handles the logic for initializing extra upgrade orbs in a room as well as
    /// setting the dungeon to the next floor, and resetting the room count.
    /// </summary>
    private void TreasureRoomLogic()
    {
        if (roomCount == (treasureRoomCount))
        {
            roomCount = 0;
            floorCount++;

            foreach (Transform child in currentRoom.transform)
            {
                child.gameObject.TryGetComponent<UpgradeOrb>(out UpgradeOrb orb);

                if (orb != null)
                {
                    Debug.Log("Initializing: " + orb.name);
                    orb.InitializeOrb(currentPlayer);
                }
            }
        }
    }
}
