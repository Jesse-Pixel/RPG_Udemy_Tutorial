using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
    
        [SerializeField] private float health = 100.0f;
        private bool _isDead = false;
        public bool IsDead
        {
            get { return _isDead; }
            set { _isDead = value; }
        }

        public void TakeDamage(float damage)
        {

            health = Mathf.Max(health - damage, 0);
            Debug.Log("Health: " + health);
            if(!_isDead && health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (IsDead) return;

            IsDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

    }
}
