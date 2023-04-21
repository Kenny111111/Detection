using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Detection
{
    enum AIState 
    {
        Default,        // Default state, should not occur.
        Patrolling,     // Ai is walking around waypoints.
        Chasing,        // Ai is running towards the player until it can attack.
        Attacking,      // Ai is attacking the player using its weapon.
        Alerted,        // Ai is walking towards a sound location.
        Dead,           // Ai is dead and not doing anything.
        Paused          // Game is paused, so the Ai is doing nothing.
    }

    public class AIController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        private Enemy enemyAI;
        [SerializeField] private AIState aiState = AIState.Default;

        [SerializeField] private float walkingSpeed = 2.50f;
        [SerializeField] private float runningSpeed = 4.0f;
        [SerializeField] private float hearingDistance = 15f;

        [SerializeField] private float aiDetectRadius = 20.0f;
        [SerializeField] private float aiViewAngle = 90.0f;

        private LayerMask obstacleLayerMask;

        [SerializeField] private List<Transform> waypoints;
        private int curWaypoint;
        private Animator animator;

        private Vector3 playerLastPosition = Vector3.zero;

        [SerializeField] private AIWeaponManager weaponManager;
        private readonly int speedHash = Animator.StringToHash("Speed");

        private WeaponInverseKinematics weaponInverseKinematics;
        private Transform playerTransform;
        private AIWeaponManager.NecessaryUseConditions aiWeaponUseConditions;
        private bool startAttack;

        void Start()
        {
            curWaypoint = 0;

            enemyAI = GetComponent<Enemy>();
            obstacleLayerMask = LayerMask.GetMask("Environment");

            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = walkingSpeed;

            if (waypoints.Count != 0) navMeshAgent.SetDestination(waypoints[curWaypoint].position);

            weaponInverseKinematics = GetComponent<WeaponInverseKinematics>();
            animator = GetComponent<Animator>();
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            aiWeaponUseConditions = weaponManager.GetWeaponNecessaryUseConditions();
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
            if (GameManager.instance.GetGameState() == GameState.LEVELPAUSED)
            {
                aiState = AIState.Paused;
                return;
            }

            // Update the animator speedHash (how fast it looks like its moving)
            if (!navMeshAgent.isStopped) animator.SetFloat(speedHash, navMeshAgent.velocity.magnitude);
            else animator.SetFloat(speedHash, 0);

            Vector3 aiPosition = transform.position;
            Vector3 playerPosition = playerTransform.position;
            // Magic number offsets :) TO FIX PLS
            aiPosition.y += 1.1f;
            playerPosition.y += 1.1f;

            Vector3 dirToPlayer = playerPosition - aiPosition;
            float distanceToPlayer = dirToPlayer.magnitude;
            dirToPlayer.Normalize();

            if (CanSeePlayerUnobstructed(aiPosition, playerPosition, distanceToPlayer, dirToPlayer))
            {
                if (CanAttackWithWeaponRequirements(distanceToPlayer)) aiState = AIState.Attacking;
                else aiState = AIState.Chasing;
            }
            else
            {
                if (aiState != AIState.Alerted) aiState = AIState.Patrolling;
            }

            switch (aiState)
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
                case AIState.Alerted:
                    break;
                default:
                    Debug.Log("Unexpected aiState");
                    break;
            }
        }

        private bool CanSeePlayerUnobstructed(Vector3 aiPosition, Vector3 playerPosition, float distanceToPlayer, Vector3 dirToPlayer)
        {
            // Ensure the player is in range. If the player is out of range, do nothing
            if (distanceToPlayer > aiDetectRadius) return false;

            // Ensure the player is in the enemies viewangle. If the player is not within the ai's viewangle, do nothing
            if (Vector3.Angle(transform.forward, dirToPlayer) > aiViewAngle / 2) return false;

            // Ensure the player is viewable. If there are any objects between the ai and the player, do nothing
            if (Physics.Raycast(aiPosition, dirToPlayer, (float)distanceToPlayer, obstacleLayerMask)) return false;

            // All logical requirements are met to 'see' a player.
            playerLastPosition = playerPosition;
            return true;
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


        private bool CanAttackWithWeaponRequirements(float distanceToPlayer)
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

        public void Alerted(Vector3 soundPos)
        {
            // Calculate distance between the AI and the sound position
            float distance = Vector3.Distance(transform.position, soundPos);

            // If the sound is within hearing range, respond to it
            if (distance <= hearingDistance)
            {
                aiState = AIState.Alerted;
                SetAiMoveSpeed(runningSpeed);
                navMeshAgent.SetDestination(soundPos);
            }
        }
    }
}
