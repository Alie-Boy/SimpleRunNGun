using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[SerializeField] float moveSpeed = 6f;
	[SerializeField] List<Transform> PatrolPoints = new List<Transform>();
	[SerializeField] float waitTime;

	private float startWaitTime;
	private int randomPatrolPoint;

	void Start()
	{
		startWaitTime = waitTime;
		randomPatrolPoint = Random.Range(0, PatrolPoints.Count);
	}

	void FixedUpdate()
	{
		transform.position = Vector3.MoveTowards(transform.position, PatrolPoints[randomPatrolPoint].position, moveSpeed * Time.fixedDeltaTime);
		if (Vector3.Distance(transform.position, PatrolPoints[randomPatrolPoint].position) < 0.2f)
		{
			if (startWaitTime <= 0f)
			{
				randomPatrolPoint = Random.Range(0, PatrolPoints.Count);
				startWaitTime = waitTime;
			} else
			{
				startWaitTime -= Time.fixedDeltaTime;
			}

		}
	}
}
