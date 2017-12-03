using UnityEngine;

namespace Assets.Scripts.Building
{
	public abstract class BaseBuilding : MonoBehaviour
	{
		public BuildingEnum Type { get; protected set; }

		public Transform Door;

		public Transform Model;

		public override string ToString() => $"[Building {Type}]";
	}
}
