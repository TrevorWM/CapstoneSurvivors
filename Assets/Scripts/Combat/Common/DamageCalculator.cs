using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    public float CalculateDamage(CharacterStatsSO ownerStats, AttackPayload payload)
    {
        bool isCrit = CriticalRoll(payload.CritChance);

        float damage = payload.Damage;

        // Multiply by critical modifier if critical, will be in the format x.x rather than percentage
        if (isCrit)
        {
            damage *= payload.CritMultiplier;
        }

        // Add payload owners elemental affinity multiplier
        // (default if none is specified is 1x so this will do nothing)
        damage *= payload.ElementalAffinity;

        // Then check for weakness/resistance interactions
        if (IsWeak(ownerStats.CharacterElement, payload.Element))
        {
            damage *= 1.25f;
        }
        else if (IsResistant(ownerStats.CharacterElement, payload.Element)) 
        {
            damage *= 0.75f;
        }

        // Then subtract reciever defence
        damage -= ownerStats.Defense;

        // Finally, take the damage multiplier into consideration
        damage *= payload.DamageMultiplier;

        Debug.Log("Damage: " + damage);

        return damage;
    }

    /// <summary>
    /// Given a chance in percentage, we need to roll if it hits the percentage or not...
    /// </summary>
    /// <param name="criticalChance"> A percentage chance of crit </param>
    /// <returns> If it was a crit or not </returns>
    private bool CriticalRoll(float criticalChance)
    {
        int randRoll = UnityEngine.Random.Range(1, 101);

        if (randRoll <= criticalChance)
        {
            //do something fancy for crits
            return true;
        }

        return false;
    }

    private bool IsWeak(ElementType ownerElement, ElementType payloadElement)
    {
        if (ownerElement == ElementType.None || payloadElement == ElementType.None)
        {
            return false;
        }
        else if (ownerElement == ElementType.Fire && payloadElement == ElementType.Water)
        {
            return true;
        }
        else if (ownerElement == ElementType.Nature && payloadElement == ElementType.Fire)
        {
            return true;
        }
        else if (ownerElement == ElementType.Water && payloadElement == ElementType.Nature)
        {
            return true;
        }
        return false;
    }

    private bool IsResistant(ElementType ownerElement, ElementType payloadElement)
    {
        if (ownerElement == ElementType.None || payloadElement == ElementType.None)
        {
            return false;
        }
        else if (ownerElement == ElementType.Water && payloadElement == ElementType.Fire)
        {
            return true;
        }
        else if (ownerElement == ElementType.Fire && payloadElement == ElementType.Nature)
        {
            return true;
        }
        else if (ownerElement == ElementType.Nature && payloadElement == ElementType.Water)
        {
            return true;
        }
        return false;
    }
}
