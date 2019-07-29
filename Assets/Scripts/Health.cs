using UnityEngine;

public class Health : MonoBehaviour {

	[SerializeField] int healthPoints = 100;

	public void TakeDamage(int damage)
	{
		healthPoints -= damage;
		// evaluating death condition only on taking damge instead of per frame tremendously reduces unnecessary load.
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

		// this bit just sets the transform in a way that the character looks dead.
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90);
		transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

		// destroying only the component and not the gameobject.
		Destroy(this);
	}
}
