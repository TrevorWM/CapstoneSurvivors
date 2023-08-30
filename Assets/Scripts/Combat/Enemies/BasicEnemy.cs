using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //Detecting Player and Obstacles around
        InvokeRepeating("PerformDetection", 0, detectionDelay);
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
        //Enemy AI movement based on Target availability
        if (aiData.currentTarget != null)
        {
            if (movementInput.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movementInput.x < 0)
            {
                spriteRenderer.flipX = false;
            }
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            //Target acquisition logic
            aiData.currentTarget = aiData.targets[0];
        }
        //Moving the enemy
        HandleMovement();
    }

    private void HandleMovement()
    {
        movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);

        if (enemyRigidbody && movementInput != null)
        {
            enemyRigidbody.velocity = movementInput * EnemyStats.MoveSpeed;
        }
    }


}
