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

    private float moveSpeed;
    
    private float dodgeForce;
    private float dodgeCooldown;
    private float dodgeDuration;
    

    private bool isDodging = false;

    private Vector2 moveVector = Vector2.zero;

    private Rigidbody2D playerRigidbody;
    private PlayerInputActions playerInputActions;
    private InputAction moveInput;
    private InputAction dodgeInput;
    private float lastDodgeTime;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        moveSpeed = playerStats.MoveSpeed;
        dodgeForce = playerStats.DodgeForce;
        dodgeCooldown = playerStats.DodgeCooldown;
        dodgeDuration = playerStats.DodgeDuration;

        
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();


        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            playerRigidbody = rigidbody;
        }
        
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        moveInput = playerInputActions.Gameplay.Move;
        dodgeInput = playerInputActions.Gameplay.Dodge;

        moveInput.Enable();
        dodgeInput.Enable();
    }

    private void OnDisable()
    {
        moveInput.Disable();
        dodgeInput.Disable();
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
            if (dodgeInput.IsPressed())
            {
                HandleDodge();
            }
            
            HandleMovement();
            
        }
    }

    /// <summary>
    /// If the object has a rigidbody this method will move them using the movespeed
    /// stat, and the current movement vector.
    /// </summary>
    private void HandleMovement()
    {
        if (playerRigidbody)
        {
            playerRigidbody.velocity = moveVector * moveSpeed;
        }
    }

    /// <summary>
    /// This method will slide the player character in the direction they are moving.
    /// Does not work if the player is not inputting a direction.
    /// </summary>
    private void HandleDodge()
    {
        if (moveVector != Vector2.zero && Time.time - lastDodgeTime >= dodgeCooldown)
        {
            isDodging = true;
            lastDodgeTime = Time.time;
            StartCoroutine("DodgeDuration");
            moveSpeed += dodgeForce;
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            //this is where we could disable the player hitbox so they have i-frames
        }
    }

    /// <summary>
    /// Handles the cooldown for the dodge ability so that the player cannot keep
    /// pressing the dodge button to infinitely gain speed.
    /// </summary>
    IEnumerator DodgeDuration()
    {
        yield return new WaitForSeconds(dodgeDuration);
        isDodging = false;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        moveSpeed -= dodgeForce;
        //and then re-enable the hitbox here

    }
}
