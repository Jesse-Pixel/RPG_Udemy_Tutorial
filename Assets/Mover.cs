using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{

    public Transform target;

    private Vector3 targetPosition;
    private NavMeshAgent navMesh;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = target.position;
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.destination = targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
