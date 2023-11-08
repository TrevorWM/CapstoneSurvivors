using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnEnemy : MonoBehaviour, IEnemyAttack
{
    [SerializeField]
    private GameObject enemyToSpawn;
    public UnityEvent<GameObject> spawnMonster;

    private BasicEnemy ownerScript;
    private Rigidbody2D enemyRigidbody;
    private CharacterStatsSO enemyStats;

    public void DoAttack(CharacterStatsSO stats = null, Vector2 aimDirection = default, Hinderance hinderance = Hinderance.None)
    {
        //yield return new WaitForSeconds(4f);
        Debug.Log("doing attack (spawn)");
        Summon();
    }

    private void Summon()
    {
        Debug.Log("spawning");
        spawnMonster?.Invoke(enemyToSpawn);
        Debug.Log("spawned");

    }

    public void Initialize(CharacterStatsSO stats, UpgradeRarity rarity = UpgradeRarity.Common)
    {
        enemyStats = stats;

        ownerScript = GetComponentInParent<BasicEnemy>();

        enemyRigidbody = GetComponentInParent<Rigidbody2D>();
    }

    public void AbilityCleanup()
    {
        throw new System.NotImplementedException();
    }
}
