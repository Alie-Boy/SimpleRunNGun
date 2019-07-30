using UnityEngine;
using UnityEngine.UI;

public class ExitGate : MonoBehaviour
{
	[SerializeField] Text winText;

	private int numOfPickupsInScene;
	private Collider collider;
	private MeshRenderer meshRenderer;

	void Start ()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		collider = GetComponent<BoxCollider>();
		winText.text = "";
		numOfPickupsInScene = FindObjectsOfType<Pickup>().Length;
	}

	public void OnePickupPicked ()
	{
		numOfPickupsInScene--;
		if (numOfPickupsInScene <= 0)
		{
			meshRenderer.enabled = false;
			collider.isTrigger = true;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		winText.text = "You Win!!";
	}
}