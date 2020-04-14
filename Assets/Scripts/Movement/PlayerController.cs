using RPG.Combat;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Control { 
    public class PlayerController : MonoBehaviour
    {
        // Start is called before the first frame update

        private Mover mover;

        void Start()
        {
            mover = GetComponent<Mover>();
        }

        // Update is called once per frame
        void Update()
        {

            if(InteractWithCombat()) return;
            InteractWithMovement();
            
        }

        private void LateUpdate()
        {
            UpdateAnimator();
        }

        private bool InteractWithCombat()
        {
           
                RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
                foreach (RaycastHit hit in hits)
                {
                    CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                    if (target == null) continue;
                    if (Input.GetMouseButtonDown(0)) {
                        GetComponent<Fighter>().Attack(target);
                    }
                    return true;
                }

            return false;
            
        }

        private bool InteractWithMovement()
        {

            RaycastHit hit = new RaycastHit();
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(hit.point);
                }
                return true;
            }

            return false;
            
        }

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void UpdateAnimator()
        {
            Vector3 localVelocity = mover.GetLocalCurrentVelocity();
            float speedZ = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardVelocity", speedZ);
        }
    }
}
