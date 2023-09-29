using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;


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
    
    public UnityEvent<DamageCalculator> enemySpawn;

    public UnityEvent<DamageCalculator> enemyDeath;

    private MeleeAttack meleeAttack;
    private AttackPayload payload;
    private bool isAttacking = false;
    private Rigidbody2D enemyRigidbody;
    private SpriteRenderer spriteRenderer;
    private float currentHealth;
    private bool runAway = false;
    private readonly float tooClose = 3f;

    private float meleeBuffer = 0.5f;

    private Vector3 flipXScale = new Vector3(-1f, 1f, 1f);


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

        //set starting health
        currentHealth = EnemyStats.MaxHealth;
        meleeAttack = GetComponentInChildren<MeleeAttack>();

        enemySpawn.Invoke(calculator);
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
            Vector3 scale;

            if (enemyRigidbody.velocity.x > 0)
            {
                if (this.gameObject.transform.localScale.x < 0)
                {
                    scale = Vector3.Scale(this.gameObject.transform.localScale, flipXScale);
                    this.gameObject.transform.localScale = scale;
                }
            }
            else if (enemyRigidbody.velocity.x < 0)
            {
                if (this.gameObject.transform.localScale.x > 0)
                {
                    scale = Vector3.Scale(this.gameObject.transform.localScale, flipXScale);
                    this.gameObject.transform.localScale = scale;
                } 
            }
            
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            // Target acquisition logic, enemy has found the player
            aiData.currentTarget = aiData.targets[0];

            // check if enemy has reached player
            float distance = Vector3.Distance(aiData.currentTarget.position, transform.position);
            if(distance <= EnemyStats.FollowDistance + meleeBuffer)
            {
                // if they have, they can try to attack
                if (EnemyStats.RangedEnemy)
                {
                    HandleRangedAttack();
                    // if a ranged enemy gets too close we want them to run away
                    if (distance < tooClose)
                    {
                        runAway = true;
                    }
                } else
                {
                    HandleMeleeAttack();
                }
            }

        }
        // Moving the enemy
        HandleMovement();

    }


    private void HandleMovement()
    {
        if (runAway)
        {
            // make them run a bit faster cause the speed is wonky
            movementInput = -getDirectionFromTarget() * 1.75f;
            StartCoroutine(StopRunning());
        }
        else
        {
            // getting movement direction
            movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
        }

        if (enemyRigidbody && movementInput != null)
        {
            enemyRigidbody.velocity = movementInput * EnemyStats.MoveSpeed;
        }
    }

    /// <summary>
    /// This coroutune makes it so setting runAway to false is delayed,
    /// otherwise it is continually swapped from true to false, slowing
    /// down the enemies movement.
    /// </summary>
    /// <returns></returns>
    IEnumerator StopRunning()
    {
        yield return new WaitForSeconds(0.75f);
        runAway = false;

    }

    private void HandleRangedAttack()
    {
        if (!isAttacking)
        {
            ProjectileBase projectile = projectilePool.GetProjectile();

            projectile.transform.position = transform.position;
            projectile.transform.rotation = transform.rotation;

            // This parents the projectiles to the room rather than the enemy
            // if we change where the enemies shoot we will need to change how this parents
            // Easiest would be to grab a reference to the dungeon room object.
            projectile.transform.parent = gameObject.transform.parent;

            Vector2 shootDirection = getDirectionFromTarget();
            int dotSeconds = 0;
            bool enemyAttack = true;
            payload = new AttackPayload(EnemyStats.BaseDamage, dotSeconds, EnemyStats.CharacterElement, EnemyStats.CriticalChance, EnemyStats.CriticalDamageMultiplier, enemyProjectile: enemyAttack);

            projectile.FireProjectile(shootDirection, EnemyStats.ProjectileSpeed, payload);

            isAttacking = true;
            StartCoroutine("BasicAttackCooldown");
        }
    }

    private void HandleMeleeAttack()
    {
        if (!isAttacking)
        {
            if (meleeAttack != null) meleeAttack.UseMeleeAttack();
            isAttacking = true;
            StartCoroutine(BasicAttackCooldown());
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
            float damage = calculator.CalculateDamage(payload, defaultOwnerStats: enemyStats);
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
        CancelInvoke();
        enemyDeath.Invoke(calculator);
        gameObject.SetActive(false);
    }
    
}
