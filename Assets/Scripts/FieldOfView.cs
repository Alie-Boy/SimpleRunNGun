using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

	public LayerMask enemyLayerMask;
	public LayerMask obstacleMask;

	public float fovAngle;
	public float fovRadius;

	[HideInInspector]
	public List<Transform> enemiesInView = new List<Transform>();


	void Start ()
	{
		StartCoroutine("FindEnemiesWithDelay", .2f);
	}

	IEnumerator FindEnemiesWithDelay (float delay) // called by string reference.
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);
			FindEnemiesInFOVRadius();
		}
	}

	void FindEnemiesInFOVRadius()
	{
		enemiesInView.Clear();
		Collider[] enemies = Physics.OverlapSphere(transform.position, fovRadius, enemyLayerMask);
		Enemy[] allEnemies = FindObjectsOfType<Enemy>();
		foreach (Enemy enemy in allEnemies)
		{
			enemy.gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
		foreach (Collider enemy in enemies)
		{
			Vector3 dirToEnemy = (enemy.transform.position - transform.position).normalized;
			if (Vector3.Angle(transform.forward, dirToEnemy) < fovAngle / 2)
			{
				float dstToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
				if (!Physics.Raycast(transform.position, dirToEnemy, dstToEnemy, obstacleMask))
				{
					enemiesInView.Add(enemy.transform);
					enemy.gameObject.GetComponent<MeshRenderer>().enabled = true;
				}
			}
		}
	}
}
