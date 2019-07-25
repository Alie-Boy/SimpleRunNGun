using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerFieldOfView))]
public class CustomFieldOfVIewEditor : Editor {
	
	void OnSceneGUI()
	{
		PlayerFieldOfView fov = (PlayerFieldOfView)target;
		Handles.color = Color.white;
		Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360f, fov.fovRadius);

		Handles.color = Color.red;
		foreach (Transform enemyTransform in fov.enemiesInView)
		{
			Handles.DrawLine(fov.transform.position, enemyTransform.position);
		}

		Vector3 angleA = fov.DirectionFromAngle(fov.transform.eulerAngles.y + (-fov.fovAngle / 2));
		Vector3 angleB = fov.DirectionFromAngle(fov.transform.eulerAngles.y + ( fov.fovAngle / 2));

		Handles.color = Color.white;
		Handles.DrawLine(fov.transform.position, fov.transform.position + angleA * fov.fovRadius);
		Handles.DrawLine(fov.transform.position, fov.transform.position + angleB * fov.fovRadius);
	}

}
