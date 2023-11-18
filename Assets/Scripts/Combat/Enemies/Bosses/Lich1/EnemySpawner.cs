using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject room;

    private void Start()
    {
        
    }

    public void SpawnEnemy(GameObject enemyObject)
    {
        room = FindAnyObjectByType<RoomLogic>().gameObject;
        GameObject enemy = Instantiate(enemyObject, room.transform);
        enemy.transform.position = this.transform.position;
        BasicEnemy enemyScript = enemy.GetComponent<BasicEnemy>();
        enemyScript?.OnSpawn();
    }
}
