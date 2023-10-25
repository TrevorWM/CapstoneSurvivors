using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttack
{
    public void DoAttack(CharacterStatsSO stats = null, Vector2 aimDirection = default(Vector2), Hinderance hinderance = Hinderance.None);
    public void Initialize(CharacterStatsSO stats, UpgradeRarity rarity = UpgradeRarity.Common);
    public void AbilityCleanup();
}
