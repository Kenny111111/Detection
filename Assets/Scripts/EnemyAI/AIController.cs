using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Detection
{
    enum AIState {
        Default,        // Default state, should not occur.
        Patrolling,     // Ai is walking around waypoints.
        Alerted,        // Ai is walking towards a sound location.
        Chasing,        // Ai is running towards the player until it can attack.
        Attacking,      // Ai is attacking the player using its weapon.
        Dead,           // Ai is dead and not doing anything.
        Paused          // Game is paused, so the Ai is doing nothing.
    }

    public class AIController : MonoBehaviour
    {
        public NavMeshAgent navMeshAgent;
        private Enemy enemyAI;

        public float walkingSpeed = 2.50f;
        public float runningSpeed = 4.0f;

        public float aiDetectRadius = 20.0f;
        public float aiViewAngle = 90.0f;

        private LayerMask playerLayerMask;
        private LayerMask obstacleLayerMask;

        public List<Transform> waypoints;
        private int curWaypoint;
        private Animator animator;

        private Vector3 playerLastPosition = Vector3.zero;

        [SerializeField] private AIWeaponManager weaponManager;
        private readonly int speedHash = Animator.StringToHash("Speed");

        private WeaponInverseKinematics weaponInverseKinematics;
        private Transform playerTransform;

        [SerializeField] private AIState aiState = AIState.Default;

        private AIWeaponManager.NecessaryUseConditions aiWeaponUseConditions;

        private double distanceToPlayer;
        private Vector3 dirToPlayer;

        private bool startAttack;
        private bool tryAttack;

        void Start()
        {
            curWaypoint = 0;

            enemyAI = GetComponent<Enemy>();
            playerLayerMask = LayerMask.NameToLayer("Player");
            obstacleLayerMask = LayerMask.NameToLayer("Environment");

            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = walkingSpeed;

            if (waypoints.Count == 0) Debug.LogError("No waypoints set.");
            else navMeshAgent.SetDestination(waypoints[curWaypoint].position);

            weaponInverseKinematics = GetComponent<WeaponInverseKinematics>();
            animator = GetComponent<Animator>();
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            aiWeaponUseConditions = weaponManager.GetWeaponNecessaryUseConditions();

            if (playerLayerMask == 0) Debug.LogError("No playerLayerMask set.");
            if (obstacleLayerMask == 0) Debug.LogError("No obstacleLayerMask set.");
        }

        void Update()
        {
            // If the enemy is dead, dont do anything...
            if (enemyAI.isAlive == false)
            {
                aiState = AIState.Dead;
                return;
            }

            // If the game state is paused, dont do anything..
            if (GameManager.instance.GetGameState() == GameState.LEVELPAUSED) return;

            // Update the animator speedHash (how fast it looks like its moving)
            if (!navMeshAgent.isStopped) animator.SetFloat(speedHash, navMeshAgent.velocity.magnitude);
            else animator.SetFloat(speedHash, 0);

            // These functions update the ai state based on certain logical requirements
            ViewEnvironment();

            if (tryAttack)
            {
                if (CanAttackWithWeaponRequirements()) aiState = AIState.Attacking;
                else aiState = AIState.Chasing;
            }

            switch(aiState)
            {
                case AIState.Patrolling:
                    Patrolling();
                    break;
                case AIState.Attacking:
                    Attack();
                    break;
                case AIState.Chasing:
                    Chasing();
                    break;
            }
        }

        private void ViewEnvironment()
        {
            Vector3 aiPosition = transform.position;
            Vector3 playerPosition = playerTransform.position;
            distanceToPlayer = (playerPosition - aiPosition).magnitude;

            // If the player is out of range, do nothing
            if (distanceToPlayer > aiDetectRadius)
            {
                aiState = AIState.Patrolling;
                return;
            }

            dirToPlayer = (playerPosition - aiPosition).normalized;

            // Ensure the player is within the bounds of the ai's viewangle
            if (Vector3.Angle(transform.forward, dirToPlayer) < aiViewAngle / 2)
            {
                // Check if there are any objects between the ai and the player
                if (!Physics.Raycast(aiPosition, dirToPlayer, (float)distanceToPlayer, obstacleLayerMask))
                {
                    tryAttack = true;
                    playerLastPosition = playerPosition;
                }
                else tryAttack = false;
            }
            else tryAttack = false;
        }

        private void Patrolling()
        {
            SetAiMoveSpeed(walkingSpeed);

            // Only set the destination if its not already set
            if (navMeshAgent.destination == null) navMeshAgent.SetDestination(waypoints[curWaypoint].position);

            // If the agent gets to the waypoint, go to the next one
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) NextWaypoint();
        }

        private void Chasing()
        {
            SetAiMoveSpeed(runningSpeed);
            navMeshAgent.SetDestination(playerLastPosition);
        }


        public void NextWaypoint()
        {
            if (waypoints.Count == 0) return;

            curWaypoint++;
            curWaypoint %= waypoints.Count;

            navMeshAgent.SetDestination(waypoints[curWaypoint].position);
        }


        private bool CanAttackWithWeaponRequirements()
        {
            if (distanceToPlayer < aiWeaponUseConditions.idealRange)
                startAttack = true;

            if (startAttack)
            {
                // if the player is in the min/max ranges to use the weapon, then the enemy is attacking.
                if (distanceToPlayer > aiWeaponUseConditions.minRange && distanceToPlayer < aiWeaponUseConditions.maxRange)
                    return true;
                else
                {
                    startAttack = false;
                    return false;
                }
            }

            return false;
        }

        private void Attack()
        {
            // Make the ai aim the weapon towards the player
            weaponInverseKinematics.SetTargetTransform(playerTransform);

            // Stop the enemy
            StopAiFromMoving();

            // Then attack
            weaponManager.DoAttack();
        }

        void SetAiMoveSpeed(float speed)
        {
            navMeshAgent.speed = speed; // maybe use a coroutine to interpolate over time?
            navMeshAgent.isStopped = false;
        }

        void StopAiFromMoving()
        {
            navMeshAgent.speed = 0; // maybe use a coroutine to lower over time?
            navMeshAgent.isStopped = true;
        }
    }
}
