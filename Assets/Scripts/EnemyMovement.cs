using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {
	
	[SerializeField] List<Transform> PatrolPoints = new List<Transform>();
	[SerializeField] float destinationTolerance = 2f;
	[SerializeField] float waitTime;

	private NavMeshAgent navMeshAgent;
	private Transform destination;
	private float startWaitTime;
	private int randomPatrolPoint;

	void Start()
	{
		startWaitTime = waitTime;
		navMeshAgent = GetComponent<NavMeshAgent>();
		SetRandomDestination();
	}

	private void SetRandomDestination()
	{
		randomPatrolPoint = UnityEngine.Random.Range(0, PatrolPoints.Count);
		destination = PatrolPoints[randomPatrolPoint];
		navMeshAgent.SetDestination(destination.position);
	}

	void Update()
	{
		Patrol();
	}

	private void Patrol()
	{
		if (Vector3.Distance(transform.position, destination.position) < destinationTolerance)
		{
			if (startWaitTime <= 0f)
			{
				startWaitTime = waitTime;
				SetRandomDestination();
			}
			else
			{
				startWaitTime -= Time.deltaTime;
			}
		}
	}
}
