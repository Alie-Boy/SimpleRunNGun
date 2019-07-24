using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class CustomFieldOfVIewEditor : Editor {
	
	void OnSceneGUI()
	{
		FieldOfView fov = (FieldOfView)target;
		Handles.color = Color.white;
		Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360f, fov.fovRadius);

		Handles.color = Color.red;
		foreach (Transform enemyTransform in fov.enemiesInView)
		{
			Handles.DrawLine(fov.transform.position, enemyTransform.position);
		}
	}

}
