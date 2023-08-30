using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private AIData aiData;

    // Timings between detections, updates, attacks
    [SerializeField]
    private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f;//, attackDelay = 1f;


    // Stuff included if we want an enemy that attacks
    /*
    [SerializeField]
    private float attackDistance = 0.5f;

    //Inputs sent from the Enemy AI to the Enemy controller
    public UnityEvent OnAttackPressed;
    */

    // these need to reference unity events in the enemy class
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;

    [SerializeField]
    private Vector2 movementInput;

    [SerializeField]
    private ContextSolver movementDirectionSolver;

    // bool if we are currently following the player or not
    bool following = false;
    

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
            //Looking at the Target
            //OnPointerInput?.Invoke(aiData.currentTarget.position);
            if (following == false)
            {
                following = true;
                StartCoroutine(ChaseAndAttack());
            }
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            //Target acquisition logic
            aiData.currentTarget = aiData.targets[0];
        }
        //Moving the enemy
        OnMovementInput?.Invoke(movementInput);
    }

    private IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            //Stopping Logic
            Debug.Log("Stopping");
            movementInput = Vector2.zero;
            following = false;
            yield break;
        }
        else
        {
            /*
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

            if (distance < attackDistance)
            {
                //Attack logic
                movementInput = Vector2.zero;
                OnAttackPressed?.Invoke();
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                // chase logic used to be here
            }
            */

            //Chase logic
            movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
            
            // aiUpdateDelay controls how often we check updates
            yield return new WaitForSeconds(aiUpdateDelay);
            StartCoroutine(ChaseAndAttack());
            

        }

    }
    
}
