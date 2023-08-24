using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private PlayerStatsSO playerStats;

    [SerializeField]
    private bool moveEnabled;

    [SerializeField]
    private ShootProjectile basicAttackScript;

    private bool isDodging = false;
    private bool isAttacking = false;

    private Vector2 moveVector = Vector2.zero;

    private Rigidbody2D playerRigidbody;
    private PlayerInputActions playerInputActions;
    private InputAction moveInput;
    private InputAction dodgeInput;
    private InputAction basicAttackInput;
    private float lastDodgeTime;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();


        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            playerRigidbody = rigidbody;
        }
        
        playerInputActions = new PlayerInputActions();
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        moveInput = playerInputActions.Gameplay.Move;
        dodgeInput = playerInputActions.Gameplay.Dodge;
        basicAttackInput = playerInputActions.Gameplay.Fire;

        moveInput.Enable();
        dodgeInput.Enable();
        basicAttackInput.Enable();
    }

    private void OnDisable()
    {
        moveInput.Disable();
        dodgeInput.Disable();
        basicAttackInput.Disable();
    }

    private void Update()
    {
        moveVector = moveInput.ReadValue<Vector2>().normalized;
        
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

    /// <summary>
    /// If the object has a rigidbody this method will move them using the playerStats.MoveSpeed
    /// stat, and the current movement vector.
    /// </summary>
    private void HandleMovement()
    {
        if (playerRigidbody)
        {
            playerRigidbody.velocity = moveVector * playerStats.MoveSpeed;
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
            if (moveVector != Vector2.zero && Time.time - lastDodgeTime >= playerStats.DodgeCooldown)
            {
                isDodging = true;
                lastDodgeTime = Time.time;
                StartCoroutine("DodgeDuration");
                playerStats.MoveSpeed += playerStats.DodgeForce;
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
        yield return new WaitForSeconds(playerStats.DodgeDuration);
        isDodging = false;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        playerStats.MoveSpeed -= playerStats.DodgeForce;
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
        yield return new WaitForSeconds(1 / playerStats.AttacksPerSecond);
        isAttacking = false;
    }

}
