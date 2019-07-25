using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour {

	[Tooltip("Bounds where the nav mesh agent can go.")]
	[Header("Patrol")]
	[SerializeField] float minX;
	[SerializeField] float maxX;
	[SerializeField] float minZ;
	[SerializeField] float maxZ;

	[Space]
	[SerializeField] float fovAngle;
	[SerializeField] float fovRadius;
	[SerializeField] LayerMask playerMask;

	[Space]
	[SerializeField] float waitOnDestination;
	[SerializeField] float destinationTolerance;
	[Tooltip("Player Tolerance should always be lower than FOV Radius")]
	[SerializeField] float playerTolerance;

	private float waitTime;
	private bool canSeeEnemy  = false;
	private bool canHearEnemy = false;
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
		waitTime = waitOnDestination;
		randomPatrolSpot = new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ));
		navMeshAgent.SetDestination(randomPatrolSpot);
		isPatrolling = true;
	}

	void Update()
	{
		SetState();
		if (canSeeEnemy)
		{
			Chase();
		}
		if (canHearEnemy)
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
		if (Vector3.Distance(GetPlayerPostion(), transform.position) <= fovRadius)
		{
			Vector3 dirToPlayer = (GetPlayerPostion() - transform.position).normalized;
			if (Input.GetKey(KeyCode.LeftShift))
			{
				isPatrolling = false;
				canHearEnemy = true;
			}
			if (Vector3.Angle(transform.forward, dirToPlayer) < fovAngle / 2)
			{
				Ray ray = new Ray(transform.position, dirToPlayer);
				if (Physics.Raycast(ray, 100f, playerMask))
				{
					isPatrolling = false;
					canHearEnemy = false;
					canSeeEnemy = true;
				}
			}
		}
	}

	private void Investigate()
	{
		navMeshAgent.SetDestination(GetPlayerPostion());
		canHearEnemy = false;
	}

	private void Chase()
	{
		Vector3 playerPosition = GetPlayerPostion();
		Vector3 dirToPlayer = (playerPosition - transform.position).normalized;
		transform.LookAt(playerPosition);
		if (Vector3.Distance(transform.position, playerPosition) > fovRadius)
		{
			navMeshAgent.ResetPath();
			canSeeEnemy = false;
		}
		if (Vector3.Distance(transform.position, playerPosition) > playerTolerance) { 
			navMeshAgent.SetDestination(playerPosition - dirToPlayer * playerTolerance);
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
