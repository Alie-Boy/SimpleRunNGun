using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLock : MonoBehaviour {

	[SerializeField] GameObject key;

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject == key)
		{
			Destroy(gameObject);
			Destroy(collision.gameObject);
		}
	}
}
