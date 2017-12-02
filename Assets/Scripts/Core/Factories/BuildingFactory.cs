using System;
using System.Linq;

using UnityEngine;

namespace Assets.Scripts
{
	[CreateAssetMenu(menuName = "Factory/Building")]
	public class BuildingFactory : ScriptableObject
	{
		public BuildingConfig[] BuildingConfigs;

		public GameObject GetPrefab(BuildingEnum id)
		{
			BuildingConfig config = BuildingConfigs.FirstOrDefault(x => x.Id == id);

			if (config == null)
			{
				throw new NullReferenceException($"No Building config for id: {nameof(id)}");
			}

			return config.Prefab;
		}

	}

	[Serializable]
	public class BuildingConfig
	{
		public BuildingEnum Id;
		public GameObject Prefab;
	}
}
