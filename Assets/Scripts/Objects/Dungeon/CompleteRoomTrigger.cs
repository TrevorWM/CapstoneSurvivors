using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CompleteRoomTrigger : MonoBehaviour
{
    public UnityEvent completeRoom;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            completeRoom.Invoke();
        }
    }
}
