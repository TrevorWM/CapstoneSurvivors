using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData : MonoBehaviour
{
    // targets detected by our AI (list in case we want to add more)
    public List<Transform> targets = null;
    // store all obstacles around enemy they need to avoid
    public Collider2D[] obstacles = null;

    // current target to follow
    public Transform currentTarget;

    [SerializeField]
    private bool isConfused = false;

    public bool IsConfused { get => isConfused; }

    public int GetTargetsCount() => targets == null ? 0 : targets.Count;
    public void ConfuseEnemy(float duration)
    {
        if (!isConfused)
        {
            isConfused = true;
            StartCoroutine(EndConfusion(duration));
        }
    }

    private IEnumerator EndConfusion(float duration)
    {
        yield return new WaitForSeconds(duration);
        isConfused = false;
    }
}
