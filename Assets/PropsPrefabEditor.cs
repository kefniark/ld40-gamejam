using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PropPrefab))]
public class PropsPrefabEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		var component = (PropPrefab)target;

		if (GUILayout.Button("Randomize"))
		{
			Randomize(component);
		}

		if (GUILayout.Button("Randomize All"))
		{
			var targets = GameObject.FindObjectsOfType<PropPrefab>();
			foreach (var target in targets)
			{
				Randomize(target);
			}
		}

		if (GUILayout.Button("Reset"))
		{
			Reset(component);
		}

		if (GUILayout.Button("Reset All"))
		{
			var targets = GameObject.FindObjectsOfType<PropPrefab>();
			foreach (var target in targets)
			{
				Reset(target);
			}
		}
	}

	private void Randomize(PropPrefab prefab)
	{
		float x = Random.Range(-prefab.DeltaPosition.x, prefab.DeltaPosition.x);
		float y = Random.Range(-prefab.DeltaPosition.y, prefab.DeltaPosition.y);
		float z = Random.Range(-prefab.DeltaPosition.z, prefab.DeltaPosition.z);
		prefab.Container.transform.localPosition = new Vector3(x, y, z);

		float scalex = Random.Range(1f - prefab.DeltaScale.x, 1f + prefab.DeltaScale.x);
		float scaley = Random.Range(1f - prefab.DeltaScale.y, 1f + prefab.DeltaScale.y);
		float scalez = Random.Range(1f - prefab.DeltaScale.z, 1f + prefab.DeltaScale.z);
		prefab.Container.transform.localScale = new Vector3(scalex, scaley, scalez);
		
		prefab.Container.transform.localEulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
	}

	private void Reset(PropPrefab prefab)
	{
		prefab.Container.transform.localPosition = new Vector3(0, 0, 0);
		prefab.Container.transform.localScale = new Vector3(1, 1, 1);
		prefab.Container.transform.localEulerAngles = new Vector3(0, 0, 0);
	}
}
