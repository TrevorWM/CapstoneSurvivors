using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Much of this code is adapted from: https://github.com/SunnyValleyStudio/Unity-2D-Context-steering-AI
// along with all the helper files AIData.cs, ContextSolver.cs, Detector.cs, ObstacleAvoidanceBehaviour.cs,
// ObstacleDetector.cs, SeekBehaviour.cs, SteeringBehaviour.cs, and TargetDetector.cs

public class BasicEnemy : MonoBehaviour
{
    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private AIData aiData;

    // Timings between detections
    [SerializeField]
    private float detectionDelay = 0.05f;

    [SerializeField]
    private Vector2 movementInput;

    [SerializeField]
    private ContextSolver movementDirectionSolver;

    [SerializeField]
    private CharacterStatsSO enemyStats;

    private bool rightFacingSprite;

    private Vector2 targetDirection;

    private Rigidbody2D enemyRigidbody;

    private SpriteRenderer spriteRenderer;

    public CharacterStatsSO EnemyStats { get => enemyStats; private set => enemyStats = value; }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            enemyRigidbody = rigidbody;
        }
    }

    private void Start()
    {
        // Detecting Player and Obstacles around
        InvokeRepeating("PerformDetection", 0, detectionDelay);

        rightFacingSprite = EnemyStats.RightFacingSprite;
    }

    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData);
        }
    }

    private void Update()
    {
        // Enemy AI movement based on Target availability
        if (aiData.currentTarget != null)
        {
            if (movementInput.x > 0)
            {
                spriteRenderer.flipX = !rightFacingSprite;
            }
            else if (movementInput.x < 0)
            {
                spriteRenderer.flipX = rightFacingSprite;
            }
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            // Target acquisition logic
            aiData.currentTarget = aiData.targets[0];
        }
        // Moving the enemy
        HandleMovement();
    }

    private void HandleMovement()
    {
        // getting movement direction
        movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);

        if (enemyRigidbody && movementInput != null)
        {
            enemyRigidbody.velocity = movementInput * EnemyStats.MoveSpeed;
        }
    }

    /// <summary>
    /// This function defo needs a rework BUT I am dumb so I cant figure out how to make it prettier.
    /// ANYWAY, this function returns the direction of the player from the enemies point of view
    /// 
    /// The idea is that this can be used to get the direction the enemy is aiming at to shoot at the player
    /// </summary>
    /// <returns>Vector2 - the direction of the player from the enemy</returns>
    private Vector2 getDirectionFromTarget()
    {
        foreach (Detector detector in detectors)
        {
            if (detector is TargetDetector td)
            {
                Debug.Log(td.Direction);

                return td.Direction;
            }
        }
        return Vector2.zero;
    }


}
