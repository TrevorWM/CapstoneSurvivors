using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonDoor : MonoBehaviour
{
    [SerializeField]
    private CharacterStatsSO bossStats;

    [SerializeField]
    private Transform targetPosition;

    [SerializeField]
    private Transform[] bossShootPositions;


    private void Start()
    {
        targetPosition = Physics2D.OverlapCircle(transform.position, bossStats.DetectionRadius, LayerMask.NameToLayer("Player")).transform;
    }

    private void Update()
    {
        
    }
}
