using UnityEngine;

public class PlayerController : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Pickup"))
		{
			Destroy(other.gameObject);
		}
	}

	void Update()
	{
		HandleInput();
	}

	private void HandleInput()
	{
		if (Input.GetMouseButton(0))
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
			{
				if (hit.collider.gameObject.CompareTag("Key"))
				{
					GameObject key = hit.collider.gameObject;
					key.transform.position = transform.position + transform.forward * 2f;
				}
			}
		}
	}
}
