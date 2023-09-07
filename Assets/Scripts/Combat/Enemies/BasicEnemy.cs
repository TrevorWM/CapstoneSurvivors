using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// Much of this code is adapted from: https://github.com/SunnyValleyStudio/Unity-2D-Context-steering-AI
// along with all the helper files AIData.cs, ContextSolver.cs, Detector.cs, ObstacleAvoidanceBehaviour.cs,
// ObstacleDetector.cs, SeekBehaviour.cs, SteeringBehaviour.cs, and TargetDetector.cs

public class BasicEnemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;
    [SerializeField]
    private List<Detector> detectors;
    [SerializeField]
    private AIData aiData;
    [SerializeField]
    private float detectionDelay = 0.05f; // Timings between detections
    [SerializeField]
    private Vector2 movementInput;
    [SerializeField]
    private ContextSolver movementDirectionSolver;
    [SerializeField]
    private CharacterStatsSO enemyStats;
    [SerializeField]
    private ProjectilePool projectilePool;
    [SerializeField]
    private FlashSprite flashSprite;
    [SerializeField]
    private DamageCalculator calculator;

    private AttackPayload payload;
    private bool rightFacingSprite;
    private bool isAttacking = false;
    private Vector2 targetDirection;
    private Rigidbody2D enemyRigidbody;
    private SpriteRenderer spriteRenderer;
    private float currentHealth;

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

        //set starting health
        currentHealth = EnemyStats.MaxHealth;
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
            int dotSeconds = 0;
            bool enemyAttack = true;
            payload = new AttackPayload(EnemyStats.BaseDamage, dotSeconds, EnemyStats.CharacterElement, EnemyStats.CriticalChance, EnemyStats.CriticalDamageMultiplier, enemyProjectile: enemyAttack);

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

    public void TakeDamage(AttackPayload payload)
    {
        // make sure it is not an enemy projectile
        if (!payload.EnemyProjectile)
        {
            float damage = calculator.CalculateDamage(enemyStats, payload);
            currentHealth -= damage;
            flashSprite.HitFlash(spriteRenderer);
        }

        //check if enemy died from the attack
        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        // do whatever else we want
        Destroy(gameObject, 0.0f);
    }
    
}
