using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour, IDamageable
{
    [SerializeField]
    private bool moveEnabled;

    [SerializeField]
    private ShootProjectile basicAttackScript;
    [SerializeField]
    private FlashSprite flashSprite;
    [SerializeField]
    private DamageCalculator calculator;

    private bool isDodging = false;
    private bool isAttacking = false;

    private Vector2 moveVector = Vector2.zero;

    private Rigidbody2D playerRigidbody;
    private PlayerInputActions playerInputActions;
    private InputAction moveInput;
    private InputAction dodgeInput;
    private InputAction basicAttackInput;
    private InputAction interactInput;
    private float lastDodgeTime;
    private SpriteRenderer spriteRenderer;

    private IInteractable interactableInRange;

    private CharacterStats runtimeStats;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();


        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            playerRigidbody = rigidbody;
        }

        if (TryGetComponent<CharacterStats>(out CharacterStats stats))
        {
            runtimeStats = stats;
        }
    }

    private void OnEnable()
    {
        moveInput = playerInputActions.Gameplay.Move;
        dodgeInput = playerInputActions.Gameplay.Dodge;
        basicAttackInput = playerInputActions.Gameplay.Fire;
        interactInput = playerInputActions.Gameplay.Interact;

        moveInput.Enable();
        dodgeInput.Enable();
        basicAttackInput.Enable();
        interactInput.Enable();

    }

    private void OnDisable()
    {
        moveInput.Disable();
        dodgeInput.Disable();
        basicAttackInput.Disable();
        interactInput.Disable();
    }

    private void Update()
    {
        moveVector = moveInput.ReadValue<Vector2>().normalized;
        HandleInteract();

        if (moveVector.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveVector.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveEnabled && !isDodging)
        {
            HandleDodge();
            HandleMovement();
            HandleBasicAttack();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable collisionInteractable = collision.GetComponent<IInteractable>();

        if (collisionInteractable != null)
        {
            interactableInRange = collisionInteractable;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable collisionInteractable = collision.GetComponent<IInteractable>();

        if (collisionInteractable != null)
        {
            interactableInRange = null;
        }
    }

    /// <summary>
    /// If the object has a rigidbody this method will move them using the playerStats.MoveSpeed
    /// stat, and the current movement vector.
    /// </summary>
    private void HandleMovement()
    {
        if (playerRigidbody)
        {
            playerRigidbody.velocity = moveVector * runtimeStats.MoveSpeed;
        }
    }

    /// <summary>
    /// This method will slide the player character in the direction they are moving.
    /// Does not work if the player is not inputting a direction.
    /// </summary>
    private void HandleDodge()
    {
        if (dodgeInput.IsPressed())
        {
            if (moveVector != Vector2.zero && Time.time - lastDodgeTime >= runtimeStats.DodgeCooldown)
            {
                isDodging = true;
                lastDodgeTime = Time.time;
                StartCoroutine("DodgeDuration");
                runtimeStats.MoveSpeed += runtimeStats.DodgeForce;
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
                //this is where we could disable the player hitbox so they have i-frames
            }
        }
        
    }

    /// <summary>
    /// Handles the cooldown for the dodge ability so that the player cannot keep
    /// pressing the dodge button to infinitely gain speed.
    /// </summary>
    IEnumerator DodgeDuration()
    {
        yield return new WaitForSeconds(runtimeStats.DodgeDuration);
        isDodging = false;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        runtimeStats.MoveSpeed -= runtimeStats.DodgeForce;
        //and then re-enable the hitbox here

    }

    /// <summary>
    /// Basic attack logic. Checks if player is dodging or attacking and then
    /// fires a basic attack if they are not.
    /// 
    /// Holding the button down will result in the attack firing as soon as possible.
    /// Spam clicking the button should still follow the cooldown rules
    /// </summary>
    private void HandleBasicAttack()
    {
        if (basicAttackInput.IsPressed())
        {
            if (!isDodging && !isAttacking)
            {
                isAttacking = true;
                basicAttackScript.Attack();
                StartCoroutine("BasicAttackCooldown");
            }
        }   
    }

    IEnumerator BasicAttackCooldown()
    {
        yield return new WaitForSeconds(1 / runtimeStats.AttacksPerSecond);
        isAttacking = false;
    }

    /// <summary>
    /// Handles the interact logic for when the player presses the interact key.
    /// </summary>
    private void HandleInteract()
    {
        if (interactInput.WasPerformedThisFrame() && interactableInRange != null)
        {
            interactableInRange.OnInteract();
        }
        
    }

    public void TakeDamage(AttackPayload payload)
    {
        if (payload.EnemyProjectile)
        {
            float damage = calculator.CalculateDamage(payload, ownerStats: runtimeStats);
            Debug.Log("Player hit for: " + damage);
            flashSprite.HitFlash(spriteRenderer);
        }
    }
}
