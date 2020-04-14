using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {

        private NavMeshAgent navMesh;
        // Start is called before the first frame update
        void Start()
        {
            navMesh = GetComponent<NavMeshAgent>();
        }


        public void MoveTo(Vector3 destination)
        {
            navMesh.destination = destination;
            StartMovement();
        }

        public Vector3 GetCurrentVelocity()
        {
            return navMesh.velocity;
        }

        public Vector3 GetLocalCurrentVelocity()
        {
            Vector3 velocity = GetCurrentVelocity();
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            return localVelocity;
        }

        public void StopMovement()
        {
            navMesh.isStopped = true;
        }

        public void StartMovement()
        {
            navMesh.isStopped = false;
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
            
        }

        public void Cancel()
        {
            StopMovement();
        }
    }
}
