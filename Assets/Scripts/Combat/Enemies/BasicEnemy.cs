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
    private bool willRun = true;
    [SerializeField]
    private ContextSolver movementDirectionSolver;
    [SerializeField]
    private CharacterStatsSO enemyStats;
    [SerializeField]
    private FlashSprite flashSprite;
    [SerializeField]
    private DamageCalculator calculator;
    [SerializeReference]
    private GameObject attack;

    private IEnemyAttack enemyAttack;

    public UnityEvent<DamageCalculator> enemySpawn;

    public UnityEvent<DamageCalculator> enemyDeath;

    private bool isAttacking = false;
    private Rigidbody2D enemyRigidbody;
    private SpriteRenderer spriteRenderer;
    private float currentHealth;
    private bool runAway = false;
    private readonly float tooClose = 3f;

    private bool slowed = false;
    private Vector2 slowVector = new (0.3f, 0.3f);
    private bool stopped = false;
    private bool stopMoving = false;
    private Vector2 stopVector = new (0.01f, 0.01f);

    private float meleeBuffer = 0.5f;

    private Vector3 flipXScale = new Vector3(-1f, 1f, 1f);

    private Hinderance currentHinderance;
    private EnemyType enemyType;

    //Confusion stuff
    Vector2 inputModifier;
    private bool confused = false;
    private bool changeDirection = true;


    public CharacterStatsSO EnemyStats { get => enemyStats; private set => enemyStats = value; }
    public bool StopMoving { get => stopMoving; set => stopMoving = value; }

    public AIData AIData { get => aiData; }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
        {
            enemyRigidbody = rigidbody;
        }
    }

    public void OnSpawn()
    {
        RoomLogic room = FindAnyObjectByType<RoomLogic>();
        if (room == null) return;

        enemySpawn.AddListener(room.EnemyAdded);
        enemyDeath.AddListener(room.EnemyRemoved);
    }

    private void Start()
    {
        // Detecting Player and Obstacles around
        InvokeRepeating("PerformDetection", 0, detectionDelay);

        //set starting health
        currentHealth = EnemyStats.MaxHealth;

        enemySpawn.Invoke(calculator);
        enemyType = enemyStats.AiType;

        enemyAttack = Instantiate(attack, gameObject.transform).GetComponent<IEnemyAttack>();
        enemyAttack.Initialize(enemyStats);

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

            if (enemyRigidbody.velocity.x > 0 && !enemyStats.OverrideVisuals)
            {
                if (this.gameObject.transform.localScale.x < 0)
                {
                    scale = Vector3.Scale(this.gameObject.transform.localScale, flipXScale);
                    this.gameObject.transform.localScale = scale;
                }
            }
            else if (enemyRigidbody.velocity.x < 0 && !enemyStats.OverrideVisuals)
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
                if (enemyType != EnemyType.Melee)
                {
                    if (!isAttacking && !StopMoving)
                    {
                        isAttacking = true;
                        enemyAttack.DoAttack(enemyStats, getDirectionFromTarget(), currentHinderance);
                        StartCoroutine(BasicAttackCooldown());
                    }
                    // if a ranged enemy gets too close we want them to run away
                    if (distance < tooClose && willRun)
                    {
                        runAway = true;
                    }
                    if (enemyType == EnemyType.Charger)
                    {
                        StopMoving = true;
                    }
                }
                else
                {
                    if (!isAttacking)
                    {
                        isAttacking = true;
                        enemyAttack.DoAttack();
                        StartCoroutine(BasicAttackCooldown());
                    }
                }
            }

        }

        if (!StopMoving)
        {
            // Moving the enemy
            HandleMovement();
        }

    }


    private void HandleMovement()
    {        
        if (runAway)
        {
            // make them run a bit faster cause the speed is wonky
            movementInput = -getDirectionFromTarget() * 1.75f;
            StartCoroutine(StopRunning());
        } else
        {
            // getting movement direction
            movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
        }

        if (confused && changeDirection)
        {
            inputModifier = movementDirectionSolver.GetRandomMoveDirection();
            StartCoroutine(ChangeConfuseDirection());
            changeDirection = false;
            movementInput *= inputModifier;
        }
        else if (confused)
        {
            movementInput *= inputModifier;
            
        }

        if (enemyRigidbody && movementInput != null)
        {
            
            if (slowed)
            {
                enemyRigidbody.velocity = movementInput * EnemyStats.MoveSpeed * slowVector;
            } else if (stopped)
            {
                enemyRigidbody.velocity = movementInput * EnemyStats.MoveSpeed * stopVector;
            }
            else
            {
                enemyRigidbody.velocity = movementInput * EnemyStats.MoveSpeed;
            }

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
            currentHinderance = payload.Hinderance;


            if (payload.Hinderance == Hinderance.Slow) // deal with specific hinderance
            {
                slowed = true;
                spriteRenderer.color = Color.blue;
                StartCoroutine(SlowTimer(payload.EffectTime));
            } else if (payload.Hinderance == Hinderance.Stop)
            {
                stopped = true;
                spriteRenderer.color = new Color(.61f,.46f,29f,1f);
                StartCoroutine(StopTimer(payload.EffectTime));
            } else if (payload.Hinderance == Hinderance.Confuse)
            {
                confused = true;
                StartCoroutine(ConfusionTimer(payload.EffectTime));
            }
            else // else just do normal hurt stuff
            {
                flashSprite.HitFlash(spriteRenderer);
            }
        }

        //check if enemy died from the attack
        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    IEnumerator SlowTimer(float time)
    {
        yield return new WaitForSeconds(time);
        slowed = false;
        spriteRenderer.color = Color.white;
        currentHinderance = Hinderance.None;
    }
    
    IEnumerator StopTimer(float time)
    {
        yield return new WaitForSeconds(time);
        stopped = false;
        spriteRenderer.color = Color.white;
        currentHinderance = Hinderance.None;
    }

    IEnumerator ConfusionTimer(float time)
    {
        yield return new WaitForSeconds(time);
        confused = false;
        currentHinderance = Hinderance.None;
    }

    IEnumerator ChangeConfuseDirection()
    {
        yield return new WaitForSeconds(0.5f);
        changeDirection = true;
    }

    private void OnDeath()
    {
        if (enemyAttack != null) enemyAttack.AbilityCleanup();
        CancelInvoke();
        enemyDeath.Invoke(calculator);
        gameObject.SetActive(false);
    }
    
    public void ChangeAttack(GameObject newAttackPrefab, EnemyType newEnemyType, UpgradeRarity newRarity = UpgradeRarity.Common)
    {
        enemyAttack = Instantiate(newAttackPrefab, gameObject.transform).GetComponent<IEnemyAttack>();
        enemyAttack.Initialize(enemyStats, newRarity);
        enemyType = newEnemyType;
        isAttacking = false;
        stopMoving = false;
    }
}


public enum EnemyType
{
    Melee = 1,
    Ranged = 2,
    Charger = 3,
}
