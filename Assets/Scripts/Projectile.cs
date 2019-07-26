using UnityEngine;

public class Projectile : MonoBehaviour {

	[SerializeField] float travelSpeed = 6f;
	[SerializeField] float lifeTime = 2f;

	private Vector3 velocity;

	void Start()
	{
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

	void FixedUpdate()
	{
		transform.position += velocity * Time.fixedDeltaTime;
	}

}
