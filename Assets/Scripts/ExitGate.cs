using UnityEngine;

public class ExitGate : MonoBehaviour
{

	private int numOfPickupsInScene;

	void Start ()
	{
		numOfPickupsInScene = FindObjectsOfType<Pickup>().Length;
	}

	public void OnePickupPicked ()
	{
		numOfPickupsInScene--;
		if (numOfPickupsInScene <= 0)
		{
			Destroy(gameObject);
		}
	}
}