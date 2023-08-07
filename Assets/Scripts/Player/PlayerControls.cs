using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private bool moveEnabled = true;
    [SerializeField]
    private float dodgeForce = 1.05f;
    [SerializeField]
    private float dodgeCooldown = 0.3f;


    private bool isDodging = false;

    private Vector2 moveVector = Vector2.zero;

    private Rigidbody2D playerRigidbody;
    private PlayerInputActions playerInputActions;
    private InputAction moveInput;
    private InputAction dodgeInput;

    private void Awake()
    {
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
        if (moveVector != Vector2.zero)
        {
            isDodging = true;
            StartCoroutine("DodgeCooldown");
            playerRigidbody.AddForce(playerRigidbody.velocity * dodgeForce, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Handles the cooldown for the dodge ability so that the player cannot keep
    /// pressing the dodge button to infinitely gain speed.
    /// </summary>
    IEnumerator DodgeCooldown()
    {
        yield return new WaitForSeconds(dodgeCooldown);
        playerRigidbody.velocity = moveVector * moveSpeed;
        isDodging = false;

    }
}
