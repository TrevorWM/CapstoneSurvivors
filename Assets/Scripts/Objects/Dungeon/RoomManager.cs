using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    [SerializeField]
    private int roomCount;

    [SerializeField]
    private int floorCount;

    [SerializeField, SerializeReference]
    private GameObject[] tutorialRoomPool;

    [SerializeField, SerializeReference]
    private GameObject[] roomPool;

    [SerializeField, SerializeReference]
    private GameObject[] level2TutorialRoomPool;

    [SerializeField, SerializeReference]
    private GameObject[] level2RoomPool;

    [SerializeField, SerializeReference]
    private GameObject[] level3TutorialRoomPool;

    [SerializeField, SerializeReference]
    private GameObject[] level3RoomPool;

    [SerializeField, SerializeReference]
    private GameObject[] bossRoomPool;

    [SerializeField, SerializeReference]
    private GameObject[] BonusRoomPool;

    private GameObject currentRoom;
    private IDungeonRoom currentRoomLogic;
    private GameObject nextRoom;
    private int previousRoomIndex;
    private bool tutorial1Complete = false;
    private bool tutorial2Complete = false;
    private bool tutorial3Complete = false;

    private GameObject currentPlayer;
    private GameObject currentUpgradeOrb;
    private UpgradeOrb upgradeOrbLogic;

    
    
    private int treasureRoomCount;
    public GameObject CurrentRoom { get => currentRoom; }

    private void OnValidate()
    {
        roomsBeforeBoss = Mathf.Max(3, roomsBeforeBoss);
    }

    public void Awake()
    {
        //roomCount = 0;
        //floorCount = 0;
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

      
        if (!tutorial1Complete)
        {
            if (floorCount == 0 && roomCount == 1) nextRoom = tutorialRoomPool[0];
            else if (floorCount == 0 && roomCount == 2)
            {
                nextRoom = tutorialRoomPool[1];
                tutorial1Complete = true;
            }
            return nextRoom;
        }
        if (!tutorial2Complete && floorCount == 1)
        {
            if (floorCount == 1 && roomCount == 1)
            {
                nextRoom = level2TutorialRoomPool[0];
                tutorial2Complete = true;
            }
            return nextRoom;
        }
        if (!tutorial3Complete && floorCount == 2)
        {
            if (floorCount == 2 && roomCount == 1)
            {
                nextRoom = level3TutorialRoomPool[0];
                tutorial3Complete = true;
            }
            return nextRoom;
        }


        if (roomCount % (treasureRoomCount) == 0)
        {
            roomIndex = 0;
            nextRoom = BonusRoomPool[floorCount];
        }
        else if (roomCount % roomsBeforeBoss == 0)
        {
            roomIndex = floorCount;
            if (floorCount >= bossRoomPool.Length) roomIndex = floorCount % bossRoomPool.Length;
            nextRoom = bossRoomPool[roomIndex];
        }
        else if (floorCount == 0)
        {
            roomIndex = RollRoomWithDuplicateProtection(roomPool);
            nextRoom = roomPool[roomIndex];
        }
        else if (floorCount == 1)
        {
            roomIndex = RollRoomWithDuplicateProtection(level2RoomPool);
            nextRoom = level2RoomPool[roomIndex];
        }
        else if (floorCount == 2)
        {
            roomIndex = RollRoomWithDuplicateProtection(level3RoomPool);
            nextRoom = level3RoomPool[roomIndex];
        }
        else // only after player has beaten all levels, so just give them a random room from any level
        {
            (nextRoom, roomIndex) = RollRandomRoom();
        }
        previousRoomIndex = roomIndex;
        return nextRoom;
    }


    private (GameObject, int) RollRandomRoom()
    {
        int randFloor = UnityEngine.Random.Range(0, 3);
        int roomIndex = 0;
        GameObject room = null;

        switch (randFloor)
        {
            case 0:
                roomIndex = RollRoomWithDuplicateProtection(roomPool);
                room = roomPool[roomIndex];
                break;
            case 1:
                roomIndex = RollRoomWithDuplicateProtection(level2RoomPool);
                room = level2RoomPool[roomIndex];
                break;
            case 2:
                roomIndex = RollRoomWithDuplicateProtection(level3RoomPool);
                room = level3RoomPool[roomIndex];
                break;
        }

        return (room, roomIndex);
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
                    orb.InitializeOrb(currentPlayer);
                }
            }
        }
    }

    private int RollRoomWithDuplicateProtection(GameObject[] roomPool)
    {
        int roomIndex = UnityEngine.Random.Range(0, roomPool.Length);

        while (roomIndex == previousRoomIndex)
        {
            roomIndex = UnityEngine.Random.Range(0, roomPool.Length);
        }
        return roomIndex;
    }
}
