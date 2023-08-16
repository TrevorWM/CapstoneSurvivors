using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    /*
     * Events that the health component emits for things like UI, and Game State.
     * Can be subscribed to in order to trigger functions on other objects when they
     * are invoked on this script.
     * 
     * Prevents healthbars from having to check HP values every frame, and instead only
     * updates them when the object has been dealt damage.
    */
    public static Action<float> OnUpdateMaxHealth;
    public static Action<float> OnUpdateCurrentHealth;
    public static Action OnDeath;

    //Replace with health from PlayerStatSO when available
    [SerializeField]
    private float maximumHP;

    [SerializeField]
    private float currentHP;

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
        CurrentHP = MaximumHP;
        OnUpdateMaxHealth?.Invoke(maximumHP);
        Debug.LogFormat("Sending spawn event with {0}/{1} HP", CurrentHP, maximumHP);
    }

    /// <summary>
    /// Removes health from the object this component is attached to accounting
    /// for defense. 
    /// 
    /// Emits an Action event to let other objects know this object's HP has changed.
    /// 
    /// If health reaches 0 this function will call the HandleDeath function.
    /// </summary>
    /// <param name="damageTaken"></param>
    private void RemoveHealth(float damageTaken)
    {
        // TODO: implement defense value into this calculation once StatSO is added
        CurrentHP -= damageTaken;
        Debug.LogFormat("Took {0} damage, now at {1} HP", damageTaken, CurrentHP);
        OnUpdateCurrentHealth?.Invoke(CurrentHP);

        if (CurrentHP == 0)
        { 
            HandleDeath(); 
        }
    }

    /// <summary>
    /// Adds health the object this component is attached too. Will not heal
    /// over the maximum HP value.
    /// 
    /// Emits an Action event to let other objects know this object's HP has changed
    /// </summary>
    /// <param name="damageHealed"></param>
    private void AddHealth(float damageHealed)
    {
        CurrentHP += damageHealed;
        Debug.LogFormat("I healed for {0} and am now at {1} HP!", damageHealed, CurrentHP);
        OnUpdateCurrentHealth?.Invoke(CurrentHP);
    }

    /// <summary>
    /// Handles the logic for when the object this component is attached to loses
    /// all of its HP.
    /// 
    /// Emits an action event to let other objects know when this object has died.
    /// </summary>
    private void HandleDeath()
    {
        Debug.LogFormat("I died!");

        // replace with death logic once animations, sfx, vfx, and other things are in
        this.gameObject.SetActive(false);
        OnDeath?.Invoke();
    }
}
