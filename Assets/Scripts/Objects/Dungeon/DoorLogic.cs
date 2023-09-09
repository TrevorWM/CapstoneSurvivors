using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorLogic : MonoBehaviour
{
    private bool canOpen = false;
    public UnityEvent onDoorEnter;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canOpen)
        {
            Debug.Log("Door closed");
        }
        else
        {
            canOpen = false;
            onDoorEnter.Invoke();
        }
    }

    public void OnRoomComplete()
    {
        canOpen = true;
    }
}
