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
        [Header ("Weapon properties")]
        [SerializeField] float weaponRange = 2.0f;
        [SerializeField] float weaponDamage = 3.0f;
        [SerializeField][Range(.1f, 5.0f)] float attackDelay = 1.0f;
        
        private float timeSinceLastAttack = 0.0f;

        Health target;
        Mover mover;


        private void Start()
        {
            mover = GetComponent<Mover>();
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead) return;

            if (target != null && !GetIsInRange())
            {
                mover.MoveTo(target.transform.position);
            }
            else
            {
                mover.StopMovement();
                AttackBehavior();
            }

        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= attackDelay)
            {
                GetComponent<Animator>().ResetTrigger("cancelAttack");
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0.0f;
            }
            
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(target.transform.position, transform.position) < weaponRange;
        }

        internal void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            this.target = combatTarget.GetComponent<Health>();
        }


        public void Cancel()
        {
            target = null;
            GetComponent<Animator>().SetTrigger("cancelAttack");
        }

        void Hit()
        {
            Debug.Log("Whack!");
            if(target != null)
            {
                target.TakeDamage(weaponDamage);
            }
        }

        public bool CanAttack(CombatTarget target)
        {
            if (target == null) return false;
            Health targetHealth = target.GetComponent<Health>();

            return targetHealth != null && !targetHealth.IsDead;
        }


    }
}
