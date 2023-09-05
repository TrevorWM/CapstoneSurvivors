using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    //Replace with health from PlayerStatSO when available
    [SerializeField]
    private CharacterStatsSO characterStats;

    [SerializeField]
    private bool testing;

    public UnityEvent updateHealth;

    private float currentHP;
    private float maximumHP;

    public float CurrentHP 
    { 
        get => currentHP; 
        set => currentHP = Mathf.Clamp(value, 0, maximumHP); 
    }

    public float MaximumHP
    { 
        get => maximumHP;
    }

    // Start is called before the first frame update
    private void Start()
    {
        maximumHP = characterStats.MaxHealth;
        CurrentHP = MaximumHP;
        Debug.LogFormat("Spawning with {0}/{1} HP", CurrentHP, maximumHP);
        if (testing) InvokeRepeating("TestHP", 2, 1);
    }

    /// <summary>
    /// Removes health from the object this component is attached to accounting
    /// for defense. 
    /// 
    /// If health reaches 0 this function will call the HandleDeath function.
    /// </summary>
    /// <param name="damageTaken"></param>
    private void RemoveHealth(float damageTaken)
    {
        // TODO: implement defense value into this calculation once StatSO is added
        CurrentHP -= damageTaken;
        updateHealth?.Invoke();
        Debug.LogFormat("Took {0} damage, now at {1} HP", damageTaken, CurrentHP);

        if (CurrentHP == 0)
        { 
            HandleDeath(); 
        }
    }

    /// <summary>
    /// Adds health the object this component is attached too. Will not heal
    /// over the maximum HP value.
    /// </summary>
    /// <param name="damageHealed"></param>
    private void AddHealth(float damageHealed)
    {
        CurrentHP += damageHealed;
        updateHealth?.Invoke();
        Debug.LogFormat("I healed for {0} and am now at {1} HP!", damageHealed, CurrentHP);
    }

    /// <summary>
    /// Handles the logic for when the object this component is attached to loses
    /// all of its HP.
    /// </summary>
    private void HandleDeath()
    {
        Debug.LogFormat("I died!");

        // replace with death logic once animations, sfx, vfx, and other things are in
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Function used to test HP is working correctly. Feel free to mess
    /// with values as needed and add StartCoroutine where you wish to test.
    /// </summary>
    /// <returns>None</returns>
    private void TestHP()
    {
        RemoveHealth(MaximumHP/3);
        AddHealth(MaximumHP);
    }
}
