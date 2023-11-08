using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public void SpawnEnemy(GameObject enemyObject)
    {
        GameObject enemy = Instantiate(enemyObject, this.transform);
        BasicEnemy enemyScript = enemy.GetComponent<BasicEnemy>();
        enemyScript?.OnSpawn();
    }
}
