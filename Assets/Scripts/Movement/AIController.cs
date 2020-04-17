using RPG.Combat;
using RPG.Core;
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

        private Mover mover;
        private Fighter fighter;
        private GameObject player;
        private Health health;

        private Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        

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


            timeSinceLastSawPlayer += Time.deltaTime;
            if (player != null && InPlayerRange() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0.0f;
                AttackBehavior();
            }
            else if(timeSinceLastSawPlayer < suspicianTimer)
            {
                SuspicionBehavior();
            }
            else
            {
                GuardBehavior();
            }

        }

        private void AttackBehavior()
        {
            fighter.Attack(player);
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void GuardBehavior()
        {
            mover.StartMoveAction(guardPosition);
        }



        private void LateUpdate()
        {
            UpdateAnimator();
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
