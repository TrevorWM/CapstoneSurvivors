using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSwarmOnHit : OnHitEffect, IDamager
{
    private AttackPayload attackPayload;
    private float effectTime;

    [SerializeField]
    private GameObject beesPrefab;

    public AttackPayload GetAttackPayload()
    {
        return attackPayload;
    }

    public override void ActivateEffect(AttackPayload payload, Transform hitLocation, float effectDuration)
    {
        this.transform.parent = hitLocation.parent;
        this.transform.position = hitLocation.position;
        this.attackPayload = payload;
        this.effectTime = effectDuration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            BasicEnemy enemy = collision.gameObject.GetComponentInChildren<BasicEnemy>();
            Hurtbox enemyHurtbox = collision.gameObject.GetComponentInChildren<Hurtbox>();
            if (enemy != null && enemyHurtbox != null)
            {
                GameObject bees = Instantiate(beesPrefab, enemy.transform);
                DespawnAfterTime beeDespawn = bees.GetComponent<DespawnAfterTime>();
                beeDespawn.StartTimer(effectTime);
            }
        }
    }

    public void Despawn()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
