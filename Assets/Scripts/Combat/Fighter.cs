using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2.0f;
        [SerializeField][Range(.1f, 5.0f)] float attackDelay = 1.0f;
        private float timeSinceLastAttack = 0.0f;

        Transform target;
        Mover mover;


        private void Start()
        {
            mover = GetComponent<Mover>();
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;


            if (target != null && !GetIsInRange())
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.StopMovement();
                AttackBehavior();
            }

        }

        private void AttackBehavior()
        {
            
            if(timeSinceLastAttack >= attackDelay)
            {
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0.0f;
            }
            
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(target.position, transform.position) < weaponRange;
        }

        internal void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            this.target = combatTarget.transform;
        }


        public void Cancel()
        {
            target = null;
        }

        void Hit()
        {
            Debug.Log("Whack!");
            if(target != null)
            {
                target.GetComponent<Health>().TakeDamage(5.0f);
            }
        }
    }
}
