using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnAfterTime : MonoBehaviour
{
    private float objectLifetime;
    public float ObjectLifetime { get => objectLifetime; set => objectLifetime = value; }

    private void OnValidate()
    {
        ObjectLifetime = Mathf.Max(0.1f, ObjectLifetime);
    }

    public void StartTimer(float duration)
    {
        objectLifetime = duration;
        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(ObjectLifetime);
        this.gameObject.SetActive(false);
    }
}
