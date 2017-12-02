using System;
using System.Linq;

using UnityEngine;

namespace Assets.Scripts
{
	[CreateAssetMenu(menuName = "Factory/Characters")]
	public class CharacterFactory : ScriptableObject
	{
		public CharacterConfig[] CharacterConfigs;

		public GameObject GetPrefab(CharacterEnum id)
		{
			CharacterConfig config = CharacterConfigs.FirstOrDefault(x => x.Id == id);

			if (config == null)
			{
				throw new NullReferenceException($"No Character config for id: {nameof(id)}");
			}

			return config.Prefab;
		}

	}

	[Serializable]
	public class CharacterConfig
	{
		public CharacterEnum Id;
		public GameObject Prefab;
	}
}
