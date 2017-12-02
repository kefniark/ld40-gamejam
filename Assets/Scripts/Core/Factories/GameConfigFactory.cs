using System;

using UnityEngine;

namespace Assets.Scripts
{
	[CreateAssetMenu(menuName = "Factory/GameConfig")]
	public class GameConfigFactory : ScriptableObject
	{
		public float InitMoney;
		public GameBuildingConfig[] Buildings;
	}

	[Serializable]
	public class GameBuildingConfig
	{
		public float Price;
		public BuildingEnum Type;

		public override string ToString() => $"[GameBuildingConfig {Type} {Price}]";
	}
}
