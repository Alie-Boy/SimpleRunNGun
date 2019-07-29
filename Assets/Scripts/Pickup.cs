using UnityEngine;

public class Pickup : MonoBehaviour {
	
	[SerializeField] float rotationSpeed;

	private ExitGate exitGate;

	void Awake()
	{
		exitGate = FindObjectOfType<ExitGate>();
		if (exitGate == null)
		{
			Debug.LogError("No ExitGate found.");
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			exitGate.OnePickupPicked();
			Destroy(gameObject);
		}
	}

	void FixedUpdate () {
		transform.Rotate(new Vector3(15, 30, 45) * Time.fixedDeltaTime);
	}
}
