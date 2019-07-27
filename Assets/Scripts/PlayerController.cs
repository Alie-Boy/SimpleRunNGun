using UnityEngine;

public class PlayerController : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Pickup"))
		{
			Destroy(other.gameObject);
		}
	}
}
