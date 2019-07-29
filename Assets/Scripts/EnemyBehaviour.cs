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
	private bool canSeeEnemy  = false;
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
		if (isPatrolling)
		{
			Patrol();
		}
	}

	private void SetState()
	{
		isPatrolling = true;
		canSeeEnemy = false;
		if (Vector3.Distance(GetPlayerPostion(), transform.position) <= fovRadius)
		{
			Vector3 dirToPlayer = (GetPlayerPostion() - transform.position).normalized;
			if (Vector3.Angle(transform.forward, dirToPlayer) < fovAngle / 2)
			{
				Ray ray = new Ray(transform.position, dirToPlayer);
				RaycastHit hit;
				Physics.Raycast(ray, out hit, 100f, layerMask);
				if (hit.collider.tag == "Player")
				{
					isPatrolling = false;
					canSeeEnemy = true;
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

	private void FireBullets()
	{
		if (nextFireTime < 0f)
		{
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
		navMeshAgent.SetDestination(randomPatrolSpot);
		if (Vector3.Distance(transform.position, randomPatrolSpot) < destinationTolerance)
		{
			if (waitTime <= 0f)
			{
				waitTime = waitOnDestination;
				randomPatrolSpot = new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ));
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
