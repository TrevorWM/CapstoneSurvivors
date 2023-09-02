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

    [SerializeField]
    private ProjectilePool projectilePool;

    private AttackPayload payload;

    private bool rightFacingSprite;

    private bool isAttacking = false;

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

    private void FixedUpdate()
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
            // Target acquisition logic, enemy has found the player
            aiData.currentTarget = aiData.targets[0];

            // check if enemy has reached player
            float distance = Vector3.Distance(aiData.currentTarget.position, transform.position);
            if(distance < EnemyStats.FollowDistance)
            {
                // if they have, they can try to attack
                if (EnemyStats.RangedEnemy)
                {
                    HandleRangedAttack();
                } else
                {
                    // basic melee enemy attack code here
                }
            }

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

    private void HandleRangedAttack()
    {
        if (!isAttacking)
        {
            Projectile projectile = projectilePool.GetProjectile();

            projectile.transform.position = transform.position;
            projectile.transform.rotation = transform.rotation;

            Vector2 shootDirection = getDirectionFromTarget();

            payload = new AttackPayload(EnemyStats.BaseDamage, false, 0, ElementType.None);

            projectile.FireProjectile(shootDirection, EnemyStats.ProjectileSpeed, payload);

            isAttacking = true;
            StartCoroutine("BasicAttackCooldown");
        }
    }

    IEnumerator BasicAttackCooldown()
    {
        yield return new WaitForSeconds(1 / EnemyStats.AttacksPerSecond);
        isAttacking = false;
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
                return td.Direction;
            }
        }
        return Vector2.zero;
    }


}
