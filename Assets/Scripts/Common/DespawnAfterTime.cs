using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnAfterTime : MonoBehaviour
{

    [SerializeField]
    private float objectLifetime;

    private void OnValidate()
    {
        objectLifetime = Mathf.Max(0.1f, objectLifetime);
    }

    private void OnEnable()
    {
        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(objectLifetime);
        this.gameObject.SetActive(false);
    }
}
