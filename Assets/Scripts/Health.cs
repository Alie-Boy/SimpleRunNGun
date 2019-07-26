using UnityEngine;

public class Health : MonoBehaviour {

	[SerializeField] int healthPoints = 100;

	public void TakeDamage(int damage)
	{
		healthPoints -= damage;
		if (healthPoints <= 0)
		{
			KillCharacter();
		}
	}

	private void KillCharacter()
	{
		PlayerMovement Player = GetComponent<PlayerMovement>();
		if (Player == null)
		{
			Destroy(gameObject);
		}
		Destroy(GetComponent<PlayerMovement>());
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90);
		transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
		Destroy(this);
	}
}
