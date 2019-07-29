/* Some functionalities can be moved to other (new) scripts.
 * But this script is currently simple enough that I don't
 * feel it's necessary to do that.
 */

using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour {

	[SerializeField] GameObject bullets;
	[SerializeField] float rateOfFire;

	[Tooltip("Bounds where the nav mesh agent can go.")]
	[Header("Patrol")]
	[SerializeField] float minX;
	[SerializeField] float maxX;
	[SerializeField] float minZ;
	[SerializeField] float maxZ;

	[Space]
	[SerializeField] float fovAngle;
	[SerializeField] float fovRadius;
	[SerializeField] LayerMask layerMask;

	[Space]
	[SerializeField] float waitOnDestination;
	[SerializeField] float destinationTolerance;
	[Tooltip("Player Tolerance should always be lower than FOV Radius")]
	[SerializeField] float playerTolerance;

	private float nextFireTime;
	private float waitTime;

	// only one of these states will be active in a single moment.
	private bool canSeeEnemy  = false;
	private bool isSuspicious = false;
	private bool isPatrolling = false;

	Vector3 randomPatrolSpot;

	private NavMeshAgent navMeshAgent;

	void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		if (navMeshAgent == null)
		{
			Debug.LogError("NavMeshAgent component not found on :" + gameObject.name);
		}
	}

	void Start()
	{
		if (rateOfFire == 0) rateOfFire = 1;
		nextFireTime = 1 / rateOfFire;
		waitTime = waitOnDestination;
		randomPatrolSpot = new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ));
		navMeshAgent.SetDestination(randomPatrolSpot);
		isPatrolling = true;
	}

	void Update()
	{
		AIBehaviour();
	}

	private void AIBehaviour()
	{
		SetState();
		if (canSeeEnemy)
		{
			Chase();
		}
		if (isSuspicious)
		{
			Investigate();
		}
		if (isPatrolling)
		{
			Patrol();
		}
	}

	private void SetState()
	{
		isPatrolling = true;
		//canSeeEnemy = false;
		if (Vector3.Distance(GetPlayerPostion(), transform.position) <= fovRadius)
		{
			Vector3 dirToPlayer = (GetPlayerPostion() - transform.position).normalized;
			if (Vector3.Angle(transform.forward, dirToPlayer) < fovAngle / 2)
			{
				Ray ray = new Ray(transform.position, dirToPlayer);
				RaycastHit hit;
				Physics.Raycast(ray, out hit, 100f, layerMask);
				if (canSeeEnemy && hit.collider.tag == null)
				{
				}
				if (hit.collider.tag == "Player")
				{
					isPatrolling = false;
					canSeeEnemy = true;
				}
				// if enemy could see the player in previous frame but not in this frame
				else if (canSeeEnemy)
				{
					isPatrolling = false;
					isSuspicious = true;
				}
			}
		}
	}

	private void Chase()
	{
		FireBullets();

		Vector3 playerPosition = GetPlayerPostion();
		Vector3 dirToPlayer = (playerPosition - transform.position).normalized;
		transform.LookAt(playerPosition);
		if (Vector3.Distance(transform.position, playerPosition) > playerTolerance) { 
			navMeshAgent.SetDestination(playerPosition - dirToPlayer * playerTolerance);
		}
	}

	private void Investigate()
	{
		navMeshAgent.SetDestination(GetPlayerPostion());
		waitTime = waitOnDestination;					// these 2 are set here so that after the Investigation, nav
		randomPatrolSpot = GetPlayerPostion();			// meshAgent resumes his Patrol.
		isSuspicious = false;
		canSeeEnemy = false;
	}

	private void FireBullets()
	{
		if (nextFireTime < 0f)
		{
			// Instantiating 1 unit away from the object, otherwise the projectile hits the object itself.
			Instantiate(bullets, transform.position + transform.forward * 1f, transform.rotation);
			nextFireTime = 1 / rateOfFire;
		}
		else
		{
			nextFireTime -= Time.deltaTime;
		}
	}

	private void Patrol()
	{
		if (Vector3.Distance(transform.position, randomPatrolSpot) < destinationTolerance)
		{
			if (waitTime <= 0f)
			{
				waitTime = waitOnDestination;
				randomPatrolSpot = new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ));
				navMeshAgent.SetDestination(randomPatrolSpot);
			}
			else
			{
				waitTime -= Time.deltaTime;
			}
		}
	}

	private Vector3 GetPlayerPostion()
	{
		return GameObject.FindGameObjectWithTag("Player").transform.position;
	}
}
