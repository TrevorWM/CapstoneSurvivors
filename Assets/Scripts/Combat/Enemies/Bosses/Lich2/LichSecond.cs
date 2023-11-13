using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LichSecond : MonoBehaviour, IDamageable
{
    [SerializeField]
    private CharacterStatsSO bossStats;

    [SerializeField]
    private HealthBar bossHealthBar;

    [SerializeField]
    private DamageCalculator damageCalculator;

    [SerializeReference]
    private GameObject[] activeAbilities;

    private IEnemyAttack[] enemyAttacks;

    [SerializeField]
    private GameObject leftHand;
    private SpriteRenderer leftHandRenderer;
    private BasicEnemy leftHandScript;

    [SerializeField]
    private GameObject rightHand;
    private SpriteRenderer rightHandRenderer;
    private BasicEnemy rightHandScript;

    [SerializeField]
    private Sprite leftGunSprite, leftHandSprite;

    [SerializeField]
    private Sprite rightGunSprite, rightHandSprite;

    [SerializeField]
    private Transform pivotPoint;

    private float runtimeHP = 0;
    private Transform player;
    private bool fightStarted = false;

    public UnityEvent bossSpawned;
    public UnityEvent bossDeath;

    private int phase = 1;
    private bool stopMoving = false;

    private void Start()
    {
        runtimeHP = bossStats.MaxHealth;
        bossHealthBar.gameObject.SetActive(false);
        InitializeAbilities();
        bossSpawned?.Invoke();

        leftHandScript = leftHand.GetComponent<BasicEnemy>();
        leftHandRenderer = leftHandScript.GetComponentInChildren<SpriteRenderer>();
        rightHandScript = rightHand.GetComponent<BasicEnemy>();
        rightHandRenderer = rightHandScript.GetComponentInChildren<SpriteRenderer>();
        leftHandScript.enabled = false;
        rightHandScript.enabled = false;
    }

    private void Update()
    {
        if (fightStarted && !stopMoving)
        {
            transform.RotateAround(pivotPoint.position, Vector3.forward, 20 * Time.deltaTime);
            transform.rotation = Quaternion.identity;
        }
    }
    private void InitializeAbilities()
    {
        if (activeAbilities.Length > 0)
        {
            enemyAttacks = new IEnemyAttack[activeAbilities.Length];

            for (int i = 0; i < activeAbilities.Length; i++)
            {
                enemyAttacks[i] = Instantiate(activeAbilities[i], gameObject.transform).GetComponent<IEnemyAttack>();
                enemyAttacks[i].Initialize(bossStats, UpgradeRarity.Legendary);
            }
        } 
    }

    public void StartFight(Transform playerTransform)
    {
        player = playerTransform;
        bossHealthBar.gameObject.SetActive(true);
        bossHealthBar.UpdateHealthBarValue(runtimeHP, bossStats.MaxHealth);
        if (!fightStarted) StartPhaseOne();
        fightStarted = true;
        StartCoroutine(StopMovingTimer());
    }

    private IEnumerator StopMovingTimer()
    {
        float stopMovingDelay = Random.Range(3, 6);
        
        yield return new WaitForSeconds(stopMovingDelay);
        stopMoving = true;

        yield return new WaitForSeconds(2f);
        stopMoving = false;

        StartCoroutine(StopMovingTimer());
        yield return null;
    }

    public void TakeDamage(AttackPayload payload)
    {
        if (payload.EnemyProjectile == false && fightStarted)
        {
            runtimeHP -= damageCalculator.CalculateDamage(payload, defaultOwnerStats: bossStats);
            bossHealthBar.UpdateHealthBarValue(runtimeHP, bossStats.MaxHealth);
            if (runtimeHP <= 0) HandleDeath();

            if (phase == 1 && (runtimeHP / bossStats.MaxHealth < 0.80f))
            {
                StartPhaseTwo();
            }
            else if (phase == 2 && (runtimeHP / bossStats.MaxHealth < 0.20f))
            {
                StartPhaseThree();
            }
        }
    }

    private void HandleDeath()
    {
        AttackPayload killHands = new AttackPayload(99999, 0, ElementType.None, 0, 0);
        bossDeath?.Invoke();
        leftHandScript.TakeDamage(killHands);
        rightHandScript.TakeDamage(killHands);
        this.gameObject.SetActive(false);
    }

    private void StartPhaseOne()
    {
        Debug.Log("Start Phase One!");
        leftHandScript.enabled = true;
        rightHandScript.enabled = true;
    }

    private void StartPhaseTwo()
    {
        phase = 2;
        Debug.Log("Phase Two Starting!");
        SwapLeftToFingerGun(activeAbilities[3]);
        StartCoroutine(PhaseTwoFirstSwap());
    }

    private void StartPhaseThree()
    {
        phase = 3;
        Debug.Log("Phase Three Starting!");
        SwapLeftToFingerGun(activeAbilities[1]);
        SwapRightToFingerGun(activeAbilities[1]);
    }

    private IEnumerator PhaseTwoFirstSwap()
    {
        yield return new WaitForSeconds(5f);
        SwapLeftToPalm();
        SwapRightToFingerGun(activeAbilities[3]);
        StartCoroutine(PhaseTwoSecondSwap());
        yield return null;
    }

    private IEnumerator PhaseTwoSecondSwap()
    {
        yield return new WaitForSeconds(5f);
        SwapLeftToFingerGun(activeAbilities[2]);
        SwapRightToPalm();
        yield return null;
    }

    private Vector2 GetDirectionToPlayer()
    {
        return (player.transform.position - transform.position).normalized;
    }

    private void SwapLeftToFingerGun(GameObject ability)
    {
        leftHandRenderer.sprite = leftGunSprite;
        leftHandScript.ChangeAttack(ability, EnemyType.Ranged, UpgradeRarity.Legendary);
    }

    private void SwapLeftToPalm()
    {
        leftHandRenderer.sprite = leftHandSprite;
        leftHandScript.ChangeAttack(activeAbilities[0], EnemyType.Charger);
    }

    private void SwapRightToFingerGun(GameObject ability)
    {
        rightHandRenderer.sprite = rightGunSprite;
        rightHandScript.ChangeAttack(ability, EnemyType.Ranged, UpgradeRarity.Legendary);
    }

    private void SwapRightToPalm()
    {
        rightHandRenderer.sprite = rightHandSprite;
        rightHandScript.ChangeAttack(activeAbilities[0], EnemyType.Charger, UpgradeRarity.Common);
    }
}
