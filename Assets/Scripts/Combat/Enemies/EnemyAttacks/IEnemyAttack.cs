using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttack
{
    public void DoAttack(CharacterStatsSO stats = null, Vector2 aimDirection = default(Vector2));

}
