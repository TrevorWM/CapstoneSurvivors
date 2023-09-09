using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    private bool roomComplete;

    public bool RoomComplete { get => roomComplete; set => roomComplete = value; }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!RoomComplete)
        {
            Debug.Log("Door Closed");
        }
        else
        {
            Debug.Log("Door Open");
        }
    }
}
