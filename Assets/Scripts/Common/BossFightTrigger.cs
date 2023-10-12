using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossFightTrigger : MonoBehaviour
{
    public UnityEvent<Transform> startBossFight;

    private bool fightTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && fightTriggered == false)
        {
            fightTriggered = true;
            startBossFight.Invoke(collision.transform);
            startBossFight.RemoveAllListeners();
            this.enabled = false;
        }
    }
}
