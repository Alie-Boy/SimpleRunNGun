using UnityEngine;

public class Projectile : MonoBehaviour {

	[SerializeField] float travelSpeed = 6f;
	[SerializeField] float lifeTime = 2f;
	[SerializeField] int damage = 10;

	private Vector3 velocity;
	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		velocity = transform.forward * travelSpeed;
	}

	void Update()
	{
		if (lifeTime < 0f)
		{
			Destroy(gameObject);
		}
		else
		{
			lifeTime -= Time.deltaTime;
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		Health health = collider.gameObject.GetComponent<Health>();
		if (health != null)
		{
			health.TakeDamage(damage);
		}
		Destroy(gameObject);
	}

	void FixedUpdate()
	{
		rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
	}

}
