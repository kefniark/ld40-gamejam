using UnityEngine;

namespace Assets.Scripts.Building
{
	public abstract class BaseBuilding : MonoBehaviour
	{
		public BuildingEnum Type { get; protected set; }

		public Transform Door;

		public override string ToString() => $"[Building {Type}]";
	}
}
