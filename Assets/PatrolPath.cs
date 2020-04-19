using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control {
    public class PatrolPath : MonoBehaviour
    {

        [SerializeField] float waypointRadius = .3f;

        private void OnDrawGizmos()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Gizmos.color = new Color(.7f, 0f, 0f, .8f);
                Gizmos.DrawSphere(transform.GetChild(i).position, waypointRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));
            }
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextIndex(int currentIndex)
        {
            if (currentIndex == transform.childCount - 1) return 0;
            else return currentIndex + 1;
        }
    }
}
