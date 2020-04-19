using RPG.Combat;
using RPG.Control;
using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Movement { 
    [RequireComponent(typeof(Fighter))]
    [RequireComponent(typeof(Mover))]
    public class AIController : MonoBehaviour
    {

        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicianTimer = 3f;

        [Header("Patrol Path Settings")]
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] float waypointTolerance = 1.0f;
        [SerializeField] float waypointWaitTime = 3.0f;

        private Mover mover;
        private Fighter fighter;
        private GameObject player;
        private Health health;

        private Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSpentAtWaypoint = Mathf.Infinity;

        int waypointIndex = 0;
        

        private void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");

            guardPosition = transform.position;
        }

        void Update()
        {
            if (health.IsDead) return;


            
            if (player != null && InPlayerRange() && fighter.CanAttack(player))
            {
                
                AttackBehavior();
            }
            else if(timeSinceLastSawPlayer < suspicianTimer)
            {
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }

        }

        private void LateUpdate()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            UpdateAnimator();
        }

        private void AttackBehavior()
        {
            timeSinceLastSawPlayer = 0.0f;
            fighter.Attack(player);
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(waypointIndex);
        }

        private void CycleWaypoint()
        {
            waypointIndex = patrolPath.GetNextIndex(waypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }



        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSpentAtWaypoint += Time.deltaTime;

                    if (SpentEnoughTimeAtWaypoing())
                    {
                        timeSpentAtWaypoint = 0.0f;
                        CycleWaypoint();
                    }
                }
                nextPosition = GetCurrentWaypoint();
            }
            mover.StartMoveAction(nextPosition);
        }

        private bool SpentEnoughTimeAtWaypoing()
        {
            return timeSpentAtWaypoint > waypointWaitTime;
        }

       

        private bool InPlayerRange()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
        }

        private void UpdateAnimator()
        {
            Vector3 localVelocity = mover.GetLocalCurrentVelocity();
            float speedZ = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardVelocity", speedZ);
        }

        //Called by Unity in Editor.
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
