using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.FilterWindow;

public class DamageCalculator : MonoBehaviour
{
    public float CalculateDamage(AttackPayload payload, CharacterStats ownerStats = null, CharacterStatsSO defaultOwnerStats = null)
    {
        ElementType characterElement;
        float characterDefence;

        if (ownerStats)
        {
            characterElement = ownerStats.CharacterElement;
            characterDefence = ownerStats.Defense;
        }
        else
        {
            characterElement = defaultOwnerStats.CharacterElement;
            characterDefence = defaultOwnerStats.Defense;
        }

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
        if (IsWeak(characterElement, payload.Element))
        {
            damage *= 1.25f;
        }
        else if (IsResistant(characterElement, payload.Element)) 
        {
            damage *= 0.75f;
        }

        // Then subtract reciever defence
        damage -= characterDefence;

        // Finally, take the damage multiplier into consideration
        damage *= payload.DamageMultiplier;

        if (damage < 1)
        {
            damage = 1;
        }

        LogDamage(payload, damage, isCrit);
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

    private void LogDamage(AttackPayload payload, float calculatedDamage, bool crit)
    {
        Debug.LogFormat("=== Attack Payload Info ===\n" +
            "Final Damage:\t\t{9}\n" +
            "Critical:\t\t\t{8}\n" +
            "------\n" +
            "Base Damage:\t\t{0}\n" +
            "DoT Time:\t\t{1}s\n" +
            "Element:\t\t{2}\n" +
            "Crit Chance:\t\t{3}%\n" +
            "Crit Multiplier:\t\t{4}x\n" +
            "Damage Multiplier:\t{5}x\n" +
            "Affinity Multiplier:\t{6}x\n" +
            "Enemy Shot:\t\t{7}\n",
            payload.Damage, payload.DotSeconds, payload.Element,
            payload.CritChance, payload.CritMultiplier, payload.DamageMultiplier,
            payload.ElementalAffinity, payload.EnemyProjectile, crit, calculatedDamage);
    }
}
