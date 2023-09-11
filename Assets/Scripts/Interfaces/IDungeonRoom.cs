using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDungeonRoom
{
    public void OpenDoor();
    public Vector3 GetPlayerStartPosition();
    public Vector3 GetUpgradeOrbPosition();

}
